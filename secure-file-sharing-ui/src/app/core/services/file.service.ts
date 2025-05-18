import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class FileService {
  private baseUrl = 'https://localhost:7274/api/file';

  constructor(private http: HttpClient) {}

  listFiles() {
    return this.http.get<any[]>(`${this.baseUrl}/list`);
  }
  upload(file: File) {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/upload`, formData);
  }

  download(fileId: string) {
    return this.http.get(`${this.baseUrl}/download/${fileId}`, {
      responseType: 'blob'
    });
  }

  downloadWithApiKey(fileId: string, apiKey: string) {
    return this.http.get(`${this.baseUrl}/thirdparty/download/${fileId}?apiKey=${apiKey}`, {
      responseType: 'blob',
      observe: 'response'
    });
  }
  
}
