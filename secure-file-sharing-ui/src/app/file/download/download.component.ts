import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { FileService } from '../../core/services/file.service';

@Component({
  selector: 'app-download',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './download.component.html'
})
export class DownloadComponent {
  fileId = '';
  message = '';

  constructor(private fileService: FileService, private http: HttpClient) {}

  download() {
    if (!this.fileId) {
      this.message = 'File ID is required';
      return;
    }

    this.fileService.download(this.fileId).subscribe({
        next: blob => {
          const a = document.createElement('a');
          a.href = window.URL.createObjectURL(blob);
          a.download = `downloaded-${this.fileId}.dat`;
          a.click();
        },
        error: err => this.message = 'Download failed: ' + err.error
      });
  }
}
