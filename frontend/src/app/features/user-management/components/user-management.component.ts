import { Component, OnInit, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmationService, MessageService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { RippleModule } from 'primeng/ripple';

import { User } from '../../../shared/models/user.model';
import { UserService } from '../../../services/user.service';
import { UserFormComponent } from './user-form/user-form.component';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    ToolbarModule,
    ToastModule,
    ConfirmDialogModule,
    RippleModule,
    UserFormComponent
  ],
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MessageService, ConfirmationService]
})
export class UserManagementComponent implements OnInit {
  private readonly userService = inject(UserService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  users: User[] = [];
  selectedUser: User | null = null;
  displayUserDialog = false;

  // Web Worker
  private worker: Worker | undefined;

  ngOnInit(): void {
    this.loadUsers();
    this.initializeWorker();
  }

  initializeWorker(): void {
    if (typeof Worker !== 'undefined') {
      this.worker = new Worker(new URL('../workers/csv-export.worker', import.meta.url));
        
      this.worker.onmessage = ({ data: csvData }) => {
        if (csvData) {
          this.downloadCSV(csvData);
          this.messageService.add({ 
            severity: 'success', 
            summary: 'Başarılı', 
            detail: 'Kullanıcı listesi CSV olarak başarıyla indirildi.',
            life: 5000 
          });
        } else {
           this.messageService.add({ severity: 'warn', summary: 'Uyarı', detail: 'Dışa aktarılacak kullanıcı bulunamadı.' });
        }
        console.log(`CSV data received from web worker.`);
        };

        this.worker.onerror = (error) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'CSV dışa aktarılırken bir hata oluştu.' });
        console.error('Web worker error:', error);
        };
    } else {
      console.error('Web Workers are not supported in this environment.');
    }
  }
  
  exportUsersToCsv(): void {
    if (this.worker) {
      if (this.users && this.users.length > 0) {
        this.messageService.add({ severity: 'info', summary: 'İşlem Başlatıldı', detail: 'Kullanıcı listesi CSV olarak hazırlanıyor...' });
        this.worker.postMessage(this.users);
      } else {
        this.messageService.add({ severity: 'warn', summary: 'Uyarı', detail: 'Dışa aktarılacak kullanıcı bulunamadı.' });
      }
    } else {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'CSV dışa aktarma servisi başlatılamadı. Tarayıcınız desteklemiyor olabilir.' });
    }
  }

  private downloadCSV(csvData: string): void {
    const blob = new Blob([`\uFEFF${csvData}`], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', 'kullanicilar.csv');
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe(
      (users) => {
        this.users = users;
      },
      () => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcılar yüklenirken bir hata oluştu.' });
      }
    );
  }

  openNewUserDialog(): void {
    this.selectedUser = null;
    this.displayUserDialog = true;
  }

  openEditUserDialog(user: User): void {
    this.selectedUser = { ...user };
    this.displayUserDialog = true;
  }

  onUserSave(user: User): void {
    if (user.id) {
      // Güncelleme
      this.userService.updateUser(user.id, user).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı başarıyla güncellendi.' });
          this.loadUsers();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı güncellenirken bir hata oluştu.' });
        }
      });
    } else {
      // Oluşturma
      this.userService.createUser(user).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı başarıyla oluşturuldu.' });
          this.loadUsers();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı oluşturulurken bir hata oluştu.' });
        }
      });
    }
    this.displayUserDialog = false;
  }

  deleteUser(user: User): void {
    this.confirmationService.confirm({
      message: `${user.adi} ${user.soyadi} adlı kullanıcıyı silmek istediğinizden emin misiniz?`,
      header: 'Kullanıcıyı Sil',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (user.id) {
          this.userService.deleteUser(user.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı başarıyla silindi.' });
              this.loadUsers();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı silinirken bir hata oluştu.' });
            }
          });
        }
      }
    });
  }
}
