import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileService } from '../../core/services/file.service';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [CommonModule], // <-- add CommonModule here to use *ngIf, *ngFor
  templateUrl: './upload.component.html'
})
export class UploadComponent {
  file?: File;
  files: any[] = [];
  message = '';
  loading = false;
  loadingFiles = false;
  downloadingFiles: Record<string, boolean> = {};

  constructor(private fileService: FileService) {}

  ngOnInit(): void {
    this.refreshFileList();
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.file = input.files[0];
    }
  }

  upload(): void {
    if (!this.file) {
      this.message = 'No file selected.';
      return;
    }

    this.loading = true;
    this.fileService.upload(this.file).subscribe({
      next: () => {
        this.message = 'Upload successful';
        this.loading = false;
        this.refreshFileList();
      },
      error: err => {
        this.message = 'Upload failed: ' + (err.error || err.message);
        this.loading = false;
      }
    });
  }

  refreshFileList() {
  this.loadingFiles = true;
  this.fileService.listFiles().subscribe({
    next: files => {
      this.files = files;
      this.loadingFiles = false;
    },
    error: err => {
      this.loadingFiles = false;
      // handle error, show message etc
    }
  });
}

  download(id: string) {
  this.downloadingFiles[id] = true;

  this.fileService.download(id).subscribe({
    next: (blob) => {
      const file = this.files.find(f => f.id === id);
      const fileName = file?.fileName || 'downloaded-file.dat';

      const a = document.createElement('a');
      a.href = URL.createObjectURL(blob);
      a.download = fileName;
      a.click();

      this.downloadingFiles[id] = false;
    },
    error: err => {
      // Handle error
      this.downloadingFiles[id] = false;
    }
  });
}
}
