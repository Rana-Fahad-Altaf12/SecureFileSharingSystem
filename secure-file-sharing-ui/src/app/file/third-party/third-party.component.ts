import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'; // <-- required for routerLink
import { HttpClient } from '@angular/common/http';
import { FileService } from '../../core/services/file.service';

@Component({
  selector: 'app-third-party',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './third-party.component.html'
})
export class ThirdPartyComponent {
  fileId: string = '';
  apiKey: string = '';
  loading: boolean = false;
  error = '';

  constructor(private fileService: FileService, private http: HttpClient) {}

  downloadWithApiKey() {
    if (!this.fileId || !this.apiKey) {
      this.error = 'Please provide File ID and API Key';
      return;
    }

    this.loading = true;
    this.fileService.downloadWithApiKey(this.fileId, this.apiKey).subscribe({
      next: (response) => {
        this.loading = false;
        const blob = response.body!;
        const contentDisposition = response.headers.get('Content-Disposition');
    
        let fileName = 'downloaded-file.dat';
    debugger;
        // Parse UTF-8 filename* first
        const fileNameUtf8Match = contentDisposition?.match(/filename\*\=UTF-8''([^;]+)/);
        if (fileNameUtf8Match?.[1]) {
          fileName = decodeURIComponent(fileNameUtf8Match[1]);
        } else {
          // Fallback to filename=
          const fileNameMatch = contentDisposition?.match(/filename="?([^"]+)"?/);
          if (fileNameMatch?.[1]) {
            fileName = fileNameMatch[1];
          }
        }
    
        const a = document.createElement('a');
        a.href = window.URL.createObjectURL(blob);
        a.download = fileName;
        a.click();
      },
      error: (err) => {
        this.loading = false;
  
        // Try to extract readable error message
        if (err.error instanceof Blob) {
          // Try to read the text content of the Blob error
          const reader = new FileReader();
          reader.onload = () => {
            try {
              const errorJson = JSON.parse(reader.result as string);
              this.error = errorJson?.error || 'Download failed';
            } catch {
              this.error = reader.result as string || 'Download failed';
            }
          };
          reader.readAsText(err.error);
        } else {
          this.error = err.error?.error || err.message || 'Download failed';
        }
      }
    });
  }
}
