import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { UploadComponent } from './file/upload/upload.component';
import { DownloadComponent } from './file/download/download.component';
import { ThirdPartyComponent } from './file/third-party/third-party.component';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'upload', component: UploadComponent, canActivate: [AuthGuard] },
  { path: 'download', component: DownloadComponent, canActivate: [AuthGuard] },
  { path: 'third-party', component: ThirdPartyComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
