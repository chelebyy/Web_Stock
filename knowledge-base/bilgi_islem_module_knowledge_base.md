# Bilgi İşlem Modülü Knowledge Base

## Modül Yapısı

Bilgi İşlem modülü, Angular feature module (özellik modülü) yaklaşımı kullanılarak aşağıdaki modüler yapıda geliştirilecektir:

```
src/app/
├── features/
│   ├── bilgi-islem/                 # Bilgi İşlem feature modülü
│   │   ├── bilgi-islem.module.ts    # Modül tanımlaması
│   │   ├── bilgi-islem-routing.module.ts  # Yönlendirme
│   │   ├── components/              # Bileşenler dizini
│   │   │   ├── bilgi-islem-list/    # Liste görünümü bileşeni
│   │   │   ├── bilgi-islem-detail/  # Detay görünümü bileşeni
│   │   │   ├── bilgi-islem-form/    # Ekleme/düzenleme formu
│   │   │   └── ...
│   │   ├── services/                # Servisler
│   │   │   ├── bilgi-islem.service.ts
│   │   │   └── ...
│   │   └── models/                  # Veri modelleri
│   │       ├── bilgi-islem.model.ts
│   │       └── ...
│   └── ...
└── ...
```

## Gerekçe

Bu modüler yapının tercih edilmesinin gerekçeleri:

1. **Modüler Yapı**: Tüm bilgi işlem ile ilgili kod bir arada ve diğer modüllerden bağımsız olur
2. **Lazy Loading**: Sadece gerektiğinde yüklenen bir modül olarak performans avantajı sağlar
3. **Bakım Kolaylığı**: Tüm ilgili dosyalar bir arada olduğu için bakımı kolaylaşır
4. **Yeniden Kullanılabilirlik**: Bileşenler modül içinde kolayca paylaşılabilir

## Backend Yapısı

Backend tarafında aşağıdaki yapı kullanılacaktır:

```
Controllers/
├── BilgiIslemController.cs  # API endpoints
Services/
├── BilgiIslemService.cs     # İş mantığı
Models/
├── BilgiIslem/              # Veri modelleri ve DTO'lar
Data/
├── Repositories/
│   ├── BilgiIslemRepository.cs  # Veritabanı işlemleri
```

## Geliştirme Adımları

### 1. Temel Yapının Oluşturulması

- [ ] `features/bilgi-islem` dizinini oluştur
- [ ] Modül dosyasını oluştur (`bilgi-islem.module.ts`)
- [ ] Routing modülünü oluştur (`bilgi-islem-routing.module.ts`)
- [ ] Diğer alt dizinleri oluştur (components, services, models)

### 2. Backend Bileşenlerinin Geliştirilmesi

- [ ] Domain katmanında Bilgi İşlem ile ilgili entity'leri oluştur
- [ ] Application katmanında DTO'ları tanımla
- [ ] Infrastructure katmanında repository'leri geliştir
- [ ] API katmanında controller'ı oluştur

### 3. Frontend Bileşenlerinin Geliştirilmesi

- [ ] Veri modellerini tanımla
- [ ] HTTP servisini oluştur
- [ ] Liste görünümü bileşenini geliştir
- [ ] Detay görünümü bileşenini geliştir
- [ ] Form bileşenini geliştir

### 4. Yönlendirme ve İzin Yapılandırması

- [ ] Ana routing modülünde Bilgi İşlem modülüne yönlendirme ekle
- [ ] Gerekli izinleri tanımla ve AuthGuard kullan
- [ ] Kullanıcı Dashboard'ına Bilgi İşlem bağlantısı ekle

### 5. Test ve Dokümantasyon

- [ ] Birim testlerini yaz
- [ ] End-to-end testlerini yaz
- [ ] Kullanıcı kılavuzunu hazırla
- [ ] API dokümantasyonunu tamamla

## Frontend Kod Şablonları

### Modül Tanımı

```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// PrimeNG bileşenleri
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';

// Routing
import { BilgiIslemRoutingModule } from './bilgi-islem-routing.module';

// Bileşenler
import { BilgiIslemListComponent } from './components/bilgi-islem-list/bilgi-islem-list.component';
import { BilgiIslemDetailComponent } from './components/bilgi-islem-detail/bilgi-islem-detail.component';
import { BilgiIslemFormComponent } from './components/bilgi-islem-form/bilgi-islem-form.component';

// Servisler
import { BilgiIslemService } from './services/bilgi-islem.service';

@NgModule({
  declarations: [
    BilgiIslemListComponent,
    BilgiIslemDetailComponent,
    BilgiIslemFormComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    BilgiIslemRoutingModule,
    
    // PrimeNG modülleri
    TableModule,
    ButtonModule,
    InputTextModule,
    CardModule,
    DialogModule,
    ConfirmDialogModule,
    ToastModule
  ],
  providers: [
    BilgiIslemService
  ],
  exports: [
    BilgiIslemListComponent
  ]
})
export class BilgiIslemModule { }
```

### Routing Modülü

```typescript
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Bileşenler
import { BilgiIslemListComponent } from './components/bilgi-islem-list/bilgi-islem-list.component';
import { BilgiIslemDetailComponent } from './components/bilgi-islem-detail/bilgi-islem-detail.component';
import { BilgiIslemFormComponent } from './components/bilgi-islem-form/bilgi-islem-form.component';

// Guard
import { AuthGuard } from '../../guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: BilgiIslemListComponent,
    canActivate: [AuthGuard],
    data: { permission: 'Pages.BilgiIslem.View' }
  },
  {
    path: 'detail/:id',
    component: BilgiIslemDetailComponent,
    canActivate: [AuthGuard],
    data: { permission: 'Pages.BilgiIslem.View' }
  },
  {
    path: 'create',
    component: BilgiIslemFormComponent,
    canActivate: [AuthGuard],
    data: { permission: 'Pages.BilgiIslem.Create' }
  },
  {
    path: 'edit/:id',
    component: BilgiIslemFormComponent,
    canActivate: [AuthGuard],
    data: { permission: 'Pages.BilgiIslem.Edit' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BilgiIslemRoutingModule { }
```

### Servis Şablonu

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

import { BilgiIslemItem } from '../models/bilgi-islem.model';

@Injectable()
export class BilgiIslemService {
  private apiUrl = `${environment.apiUrl}/api/bilgi-islem`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<BilgiIslemItem[]> {
    return this.http.get<BilgiIslemItem[]>(this.apiUrl);
  }

  getById(id: number): Observable<BilgiIslemItem> {
    return this.http.get<BilgiIslemItem>(`${this.apiUrl}/${id}`);
  }

  create(item: BilgiIslemItem): Observable<BilgiIslemItem> {
    return this.http.post<BilgiIslemItem>(this.apiUrl, item);
  }

  update(id: number, item: BilgiIslemItem): Observable<BilgiIslemItem> {
    return this.http.put<BilgiIslemItem>(`${this.apiUrl}/${id}`, item);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
```

### Model Şablonu

```typescript
export interface BilgiIslemItem {
  id?: number;
  name: string;
  description: string;
  status: string;
  priority: number;
  createdDate: Date;
  assignedTo?: number;
  assignedToName?: string;
  // Diğer özellikler...
}

export enum BilgiIslemStatus {
  Open = 'Open',
  InProgress = 'InProgress',
  Resolved = 'Resolved',
  Closed = 'Closed'
}

export enum BilgiIslemPriority {
  Low = 1,
  Medium = 2,
  High = 3,
  Critical = 4
}
```

### Liste Bileşeni Şablonu

```typescript
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService, ConfirmationService } from 'primeng/api';

import { BilgiIslemService } from '../../services/bilgi-islem.service';
import { BilgiIslemItem, BilgiIslemStatus } from '../../models/bilgi-islem.model';

@Component({
  selector: 'app-bilgi-islem-list',
  templateUrl: './bilgi-islem-list.component.html',
  styleUrls: ['./bilgi-islem-list.component.scss'],
  providers: [MessageService, ConfirmationService]
})
export class BilgiIslemListComponent implements OnInit {
  items: BilgiIslemItem[] = [];
  loading = false;
  statuses = Object.values(BilgiIslemStatus);

  constructor(
    private bilgiIslemService: BilgiIslemService,
    private router: Router,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.loading = true;
    this.bilgiIslemService.getAll().subscribe({
      next: (data) => {
        this.items = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Bilgi İşlem öğeleri yüklenirken hata oluştu', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Bilgi İşlem öğeleri yüklenirken bir hata oluştu'
        });
        this.loading = false;
      }
    });
  }

  viewDetail(id: number): void {
    this.router.navigate(['/bilgi-islem/detail', id]);
  }

  createNew(): void {
    this.router.navigate(['/bilgi-islem/create']);
  }

  editItem(id: number): void {
    this.router.navigate(['/bilgi-islem/edit', id]);
  }

  deleteItem(id: number): void {
    this.confirmationService.confirm({
      message: 'Bu öğeyi silmek istediğinizden emin misiniz?',
      header: 'Silme Onayı',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.bilgiIslemService.delete(id).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Öğe başarıyla silindi'
            });
            this.loadItems();
          },
          error: (error) => {
            console.error('Öğe silinirken hata oluştu', error);
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Öğe silinirken bir hata oluştu'
            });
          }
        });
      }
    });
  }

  getPriorityClass(priority: number): string {
    switch (priority) {
      case 1: return 'low-priority';
      case 2: return 'medium-priority';
      case 3: return 'high-priority';
      case 4: return 'critical-priority';
      default: return '';
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Open': return 'status-open';
      case 'InProgress': return 'status-progress';
      case 'Resolved': return 'status-resolved';
      case 'Closed': return 'status-closed';
      default: return '';
    }
  }
}
```

## Backend Kod Şablonları

### Controller Şablonu

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Stock.Application.Features.BilgiIslem.Commands;
using Stock.Application.Features.BilgiIslem.Queries;
using MediatR;

namespace Stock.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/bilgi-islem")]
    public class BilgiIslemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BilgiIslemController> _logger;

        public BilgiIslemController(IMediator mediator, ILogger<BilgiIslemController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "Pages.BilgiIslem.View")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm Bilgi İşlem öğeleri getiriliyor");
            var query = new GetBilgiIslemListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Pages.BilgiIslem.View")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Bilgi İşlem öğesi getiriliyor: {Id}", id);
            var query = new GetBilgiIslemByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Pages.BilgiIslem.Create")]
        public async Task<IActionResult> Create([FromBody] CreateBilgiIslemCommand command)
        {
            _logger.LogInformation("Yeni Bilgi İşlem öğesi oluşturuluyor");
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Pages.BilgiIslem.Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBilgiIslemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("URL'deki ID ile istek gövdesindeki ID eşleşmiyor");
            }

            _logger.LogInformation("Bilgi İşlem öğesi güncelleniyor: {Id}", id);
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Pages.BilgiIslem.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Bilgi İşlem öğesi siliniyor: {Id}", id);
            var command = new DeleteBilgiIslemCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
```

### DTO Şablonları

```csharp
namespace Stock.Application.Features.BilgiIslem.Dtos
{
    public class BilgiIslemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? AssignedTo { get; set; }
        public string AssignedToName { get; set; }
    }

    public class CreateBilgiIslemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public int? AssignedTo { get; set; }
    }

    public class UpdateBilgiIslemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public int? AssignedTo { get; set; }
    }
}
```

## Öğrenilen Dersler ve En İyi Uygulamalar

- Feature modülü yaklaşımı, kodun organizasyonunu ve bakımını kolaylaştırır
- Lazy loading, uygulama performansını artırır
- Tüm bileşenlerin aynı modül içinde gruplandırılması, kodun yeniden kullanılabilirliğini artırır
- İzin tabanlı yetkilendirme, güvenliği artırır

## İlerleme Durumu

- [ ] Temel modül yapısı oluşturuldu
- [ ] Backend API endpoints tanımlandı
- [ ] Frontend modül ve servisler geliştirildi
- [ ] Liste, detay ve form ekranları tamamlandı
- [ ] Yetkilendirme ve izin kontrolleri eklendi
- [ ] Testler yazıldı

## Kaynaklar

1. [Angular Feature Modules](https://angular.io/guide/feature-modules)
2. [Lazy Loading Angular Modules](https://angular.io/guide/lazy-loading-ngmodules)
3. [PrimeNG Table Component](https://www.primefaces.org/primeng/table)
4. [Clean Architecture with ASP.NET Core](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture) 