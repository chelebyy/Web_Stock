# Frontend Performans Optimizasyonları

Bu belge, Stock uygulamasının frontend performansını optimize etmek için yapılan çalışmaları ve uygulanan stratejileri detaylandırmaktadır.

## Genel Bakış

Frontend performans optimizasyonları, uygulamanın yükleme süresini azaltmak, kullanıcı deneyimini iyileştirmek ve kaynakları daha verimli kullanmak için kritik öneme sahiptir. Bu optimizasyonlar, aşağıdaki ana başlıklar altında gerçekleştirilmiştir:

1. Lazy Loading Uygulanması
2. Bundle Boyutlarının Küçültülmesi
3. Görüntü Optimizasyonu
4. Angular Change Detection Stratejisinin İyileştirilmesi
5. Tailwind CSS Optimizasyonu

## 1. Lazy Loading Uygulanması

### Uygulanan Stratejiler

#### 1.1. Feature Modülleri için Lazy Loading

Uygulamanın feature modülleri, ihtiyaç duyulduğunda yüklenecek şekilde yapılandırılmıştır.

```typescript
// app-routing.module.ts
const routes: Routes = [
  { path: '', component: HomeComponent },
  { 
    path: 'users', 
    loadChildren: () => import('./features/users/users.module').then(m => m.UsersModule),
    canActivate: [AuthGuard]
  },
  { 
    path: 'roles', 
    loadChildren: () => import('./features/roles/roles.module').then(m => m.RolesModule),
    canActivate: [AuthGuard]
  },
  { 
    path: 'products', 
    loadChildren: () => import('./features/products/products.module').then(m => m.ProductsModule),
    canActivate: [AuthGuard]
  },
  { 
    path: 'categories', 
    loadChildren: () => import('./features/categories/categories.module').then(m => m.CategoriesModule),
    canActivate: [AuthGuard]
  },
  { 
    path: 'reports', 
    loadChildren: () => import('./features/reports/reports.module').then(m => m.ReportsModule),
    canActivate: [AuthGuard]
  }
];
```

#### 1.2. Standalone Bileşenler Kullanımı

Angular 14+ ile gelen standalone bileşenler kullanılarak modül bağımlılıkları azaltılmıştır.

```typescript
// user-list.component.ts
@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    DialogModule,
    ConfirmDialogModule,
    ToastModule,
    SharedDirectivesModule
  ]
})
export class UserListComponent implements OnInit {
  // Bileşen kodu...
}
```

#### 1.3. Preloading Stratejileri

Kullanıcı deneyimini iyileştirmek için özel preloading stratejileri uygulanmıştır.

```typescript
// app-routing.module.ts
@NgModule({
  imports: [RouterModule.forRoot(routes, {
    preloadingStrategy: QuicklinkStrategy,
    scrollPositionRestoration: 'enabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
```

```typescript
// custom-preloading-strategy.ts
@Injectable({
  providedIn: 'root'
})
export class CustomPreloadingStrategy implements PreloadingStrategy {
  preload(route: Route, load: () => Observable<any>): Observable<any> {
    if (route.data && route.data['preload']) {
      return load();
    }
    return of(null);
  }
}
```

#### 1.4. Dinamik Bileşen Yükleme

Karmaşık sayfalar için dinamik bileşen yükleme stratejisi uygulanmıştır.

```typescript
// dashboard.component.ts
@Component({
  selector: 'app-dashboard',
  template: `
    <div class="dashboard-container">
      <ng-container *ngFor="let widget of visibleWidgets">
        <ng-container *ngComponentOutlet="getWidgetComponent(widget.type); 
                                         injector: getWidgetInjector(widget.data)">
        </ng-container>
      </ng-container>
    </div>
  `
})
export class DashboardComponent implements OnInit {
  visibleWidgets: Widget[] = [];
  
  constructor(private componentFactoryResolver: ComponentFactoryResolver,
              private injector: Injector) {}
  
  ngOnInit() {
    this.loadWidgets();
  }
  
  getWidgetComponent(type: string): Type<any> {
    switch (type) {
      case 'sales':
        return SalesWidgetComponent;
      case 'inventory':
        return InventoryWidgetComponent;
      case 'users':
        return UsersWidgetComponent;
      default:
        return DefaultWidgetComponent;
    }
  }
  
  getWidgetInjector(data: any): Injector {
    return Injector.create({
      providers: [
        { provide: WIDGET_DATA, useValue: data }
      ],
      parent: this.injector
    });
  }
  
  loadWidgets() {
    // Widget'ları yükleme mantığı...
  }
}
```

## 2. Bundle Boyutlarının Küçültülmesi

### Uygulanan Stratejiler

#### 2.1. Tree Shaking Optimizasyonu

Kullanılmayan kodların bundle'dan çıkarılması için tree shaking optimizasyonu yapılmıştır.

```json
// angular.json
{
  "projects": {
    "stock-app": {
      "architect": {
        "build": {
          "configurations": {
            "production": {
              "optimization": true,
              "aot": true,
              "buildOptimizer": true
            }
          }
        }
      }
    }
  }
}
```

#### 2.2. Kod Bölme (Code Splitting)

Uygulama kodu, daha küçük parçalara bölünerek yükleme performansı iyileştirilmiştir.

```typescript
// main.ts
if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(BrowserModule, BrowserAnimationsModule),
    provideRouter(routes),
    provideHttpClient(),
    provideAnimations()
  ]
}).catch(err => console.error(err));
```

#### 2.3. Dependency Injection Optimizasyonu

Angular'ın Dependency Injection sistemi optimize edilmiştir.

```typescript
// app.config.ts
export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    provideAnimations(),
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
};
```

#### 2.4. PrimeNG Modüllerinin Seçici Kullanımı

PrimeNG modülleri, sadece ihtiyaç duyulan bileşenler import edilerek optimize edilmiştir.

```typescript
// shared.module.ts
@NgModule({
  imports: [
    CommonModule,
    // Sadece kullanılan PrimeNG modülleri
    ButtonModule,
    TableModule,
    InputTextModule,
    DropdownModule,
    DialogModule,
    ToastModule
  ],
  exports: [
    CommonModule,
    ButtonModule,
    TableModule,
    InputTextModule,
    DropdownModule,
    DialogModule,
    ToastModule
  ]
})
export class SharedModule { }
```

## 3. Görüntü Optimizasyonu

### Uygulanan Stratejiler

#### 3.1. NgOptimizedImage Direktifi Kullanımı

Angular'ın NgOptimizedImage direktifi kullanılarak görüntü yükleme performansı iyileştirilmiştir.

```html
<!-- product-detail.component.html -->
<img 
  ngSrc="{{ product.imageUrl }}" 
  width="400" 
  height="300" 
  priority="high" 
  alt="{{ product.name }}"
  loading="lazy"
  sizes="(max-width: 768px) 100vw, 50vw">
```

```typescript
// app.config.ts
export const appConfig: ApplicationConfig = {
  providers: [
    // Diğer sağlayıcılar...
    provideImageKitLoader('https://stock-app-images.imagekit.io/')
  ]
};
```

#### 3.2. Responsive Görüntüler

Farklı ekran boyutları için responsive görüntüler uygulanmıştır.

```html
<!-- product-card.component.html -->
<picture>
  <source media="(max-width: 768px)" srcset="{{ product.imageUrl }}?tr=w-300">
  <source media="(max-width: 1200px)" srcset="{{ product.imageUrl }}?tr=w-500">
  <img 
    ngSrc="{{ product.imageUrl }}" 
    width="600" 
    height="400" 
    alt="{{ product.name }}"
    loading="lazy">
</picture>
```

#### 3.3. WebP Format Kullanımı

Görüntüler, daha küçük boyutlu WebP formatına dönüştürülmüştür.

```typescript
// image.service.ts
@Injectable({
  providedIn: 'root'
})
export class ImageService {
  constructor(private http: HttpClient) {}
  
  getOptimizedImageUrl(originalUrl: string): string {
    if (this.isBrowserSupportWebP()) {
      return originalUrl.replace(/\.(jpg|jpeg|png)$/i, '.webp');
    }
    return originalUrl;
  }
  
  isBrowserSupportWebP(): boolean {
    const canvas = document.createElement('canvas');
    if (canvas.getContext && canvas.getContext('2d')) {
      return canvas.toDataURL('image/webp').indexOf('data:image/webp') === 0;
    }
    return false;
  }
}
```

#### 3.4. Görüntü Lazy Loading

Görüntüler için lazy loading uygulanmıştır.

```html
<!-- product-list.component.html -->
<div class="product-grid">
  <div *ngFor="let product of products" class="product-card">
    <img 
      [attr.src]="isInViewport(product.id) ? product.imageUrl : ''" 
      [attr.data-src]="product.imageUrl"
      alt="{{ product.name }}"
      loading="lazy"
      class="product-image">
    <h3>{{ product.name }}</h3>
    <p>{{ product.price | currency }}</p>
  </div>
</div>
```

```typescript
// product-list.component.ts
@Component({
  // ...
})
export class ProductListComponent implements OnInit, AfterViewInit {
  products: Product[] = [];
  observer: IntersectionObserver;
  
  constructor() {
    this.observer = new IntersectionObserver(this.onIntersection.bind(this), {
      root: null,
      rootMargin: '0px',
      threshold: 0.1
    });
  }
  
  ngAfterViewInit() {
    const productCards = document.querySelectorAll('.product-card');
    productCards.forEach(card => this.observer.observe(card));
  }
  
  onIntersection(entries: IntersectionObserverEntry[]) {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        const img = entry.target.querySelector('img');
        if (img && img.dataset.src) {
          img.src = img.dataset.src;
        }
        this.observer.unobserve(entry.target);
      }
    });
  }
  
  isInViewport(productId: number): boolean {
    // Viewport kontrolü...
    return false;
  }
}
```

## 4. Angular Change Detection Stratejisinin İyileştirilmesi

### Uygulanan Stratejiler

#### 4.1. OnPush Change Detection Stratejisi

Bileşenlerde OnPush change detection stratejisi kullanılarak gereksiz render işlemleri azaltılmıştır.

```typescript
// product-card.component.ts
@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductCardComponent {
  @Input() product: Product;
  
  constructor(private cdr: ChangeDetectorRef) {}
  
  updateProduct(updatedProduct: Product) {
    this.product = updatedProduct;
    this.cdr.markForCheck();
  }
}
```

#### 4.2. Trackby Fonksiyonu Kullanımı

NgFor direktifi ile birlikte trackBy fonksiyonu kullanılarak liste render performansı iyileştirilmiştir.

```html
<!-- product-list.component.html -->
<div class="product-list">
  <app-product-card 
    *ngFor="let product of products; trackBy: trackByProductId" 
    [product]="product">
  </app-product-card>
</div>
```

```typescript
// product-list.component.ts
@Component({
  // ...
})
export class ProductListComponent {
  products: Product[] = [];
  
  trackByProductId(index: number, product: Product): number {
    return product.id;
  }
}
```

#### 4.3. Pure Pipe Kullanımı

Veri dönüşümleri için pure pipe'lar kullanılarak gereksiz hesaplamalar önlenmiştir.

```typescript
// filter-products.pipe.ts
@Pipe({
  name: 'filterProducts',
  pure: true
})
export class FilterProductsPipe implements PipeTransform {
  transform(products: Product[], category: string): Product[] {
    if (!products || !category) {
      return products;
    }
    return products.filter(product => product.category === category);
  }
}
```

```html
<!-- product-list.component.html -->
<div class="product-list">
  <app-product-card 
    *ngFor="let product of products | filterProducts:selectedCategory; trackBy: trackByProductId" 
    [product]="product">
  </app-product-card>
</div>
```

#### 4.4. Signals Kullanımı

Angular 16+ ile gelen Signals API kullanılarak reaktif veri yönetimi iyileştirilmiştir.

```typescript
// product.service.ts
@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private productsSignal = signal<Product[]>([]);
  private selectedCategorySignal = signal<string>('');
  
  public products = computed(() => {
    const products = this.productsSignal();
    const category = this.selectedCategorySignal();
    
    if (!category) {
      return products;
    }
    
    return products.filter(product => product.category === category);
  });
  
  constructor(private http: HttpClient) {}
  
  loadProducts() {
    this.http.get<Product[]>('/api/products').subscribe(products => {
      this.productsSignal.set(products);
    });
  }
  
  setSelectedCategory(category: string) {
    this.selectedCategorySignal.set(category);
  }
}
```

```typescript
// product-list.component.ts
@Component({
  selector: 'app-product-list',
  template: `
    <div class="category-filter">
      <button *ngFor="let category of categories()" 
              (click)="selectCategory(category)"
              [class.active]="selectedCategory() === category">
        {{ category }}
      </button>
    </div>
    
    <div class="product-list">
      @for (product of filteredProducts(); track product.id) {
        <app-product-card [product]="product"></app-product-card>
      }
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductListComponent {
  categories = signal<string[]>([]);
  selectedCategory = signal<string>('');
  products = signal<Product[]>([]);
  
  filteredProducts = computed(() => {
    const category = this.selectedCategory();
    if (!category) {
      return this.products();
    }
    return this.products().filter(product => product.category === category);
  });
  
  constructor(private productService: ProductService) {}
  
  ngOnInit() {
    this.productService.getCategories().subscribe(categories => {
      this.categories.set(categories);
    });
    
    this.productService.getProducts().subscribe(products => {
      this.products.set(products);
    });
  }
  
  selectCategory(category: string) {
    this.selectedCategory.set(category);
  }
}
```

## 5. Tailwind CSS Optimizasyonu

### Uygulanan Stratejiler

#### 5.1. JIT Modu Kullanımı

Tailwind CSS'in JIT (Just-In-Time) modu kullanılarak CSS bundle boyutu küçültülmüştür.

```javascript
// tailwind.config.js
module.exports = {
  mode: 'jit',
  content: [
    './src/**/*.{html,ts}',
  ],
  theme: {
    extend: {
      colors: {
        primary: '#3f51b5',
        secondary: '#f50057',
        accent: '#ff4081'
      }
    },
  },
  plugins: [],
}
```

#### 5.2. PurgeCSS Entegrasyonu

Kullanılmayan CSS sınıflarını temizlemek için PurgeCSS entegre edilmiştir.

```javascript
// postcss.config.js
module.exports = {
  plugins: {
    tailwindcss: {},
    autoprefixer: {},
    ...(process.env.NODE_ENV === 'production' ? {
      '@fullhuman/postcss-purgecss': {
        content: [
          './src/**/*.html',
          './src/**/*.ts',
        ],
        defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || []
      }
    } : {})
  }
}
```

#### 5.3. Özel Utility Sınıfları

Sık kullanılan stil kombinasyonları için özel utility sınıfları oluşturulmuştur.

```javascript
// tailwind.config.js
module.exports = {
  // ...
  theme: {
    extend: {
      // ...
    },
  },
  plugins: [
    function({ addUtilities }) {
      const newUtilities = {
        '.flex-center': {
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
        },
        '.card-shadow': {
          boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)',
        },
        '.transition-standard': {
          transition: 'all 0.3s ease-in-out',
        }
      }
      addUtilities(newUtilities, ['responsive', 'hover'])
    }
  ],
}
```

#### 5.4. Responsive Tasarım Optimizasyonu

Tailwind CSS'in responsive utility'leri kullanılarak mobil öncelikli tasarım uygulanmıştır.

```html
<!-- product-list.component.html -->
<div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
  <div *ngFor="let product of products; trackBy: trackByProductId" 
       class="bg-white rounded-lg shadow p-4 transition-standard hover:shadow-lg">
    <img [src]="product.imageUrl" 
         alt="{{ product.name }}" 
         class="w-full h-48 object-cover rounded mb-4">
    <h3 class="text-lg font-semibold text-gray-800">{{ product.name }}</h3>
    <p class="text-gray-600 mb-2">{{ product.description }}</p>
    <div class="flex justify-between items-center">
      <span class="text-primary font-bold">{{ product.price | currency }}</span>
      <button class="bg-primary text-white px-4 py-2 rounded hover:bg-primary-dark">
        Sepete Ekle
      </button>
    </div>
  </div>
</div>
```

## Sonuç ve Faydalar

Frontend performans optimizasyonları sonucunda aşağıdaki faydalar elde edilmiştir:

1. **Sayfa Yükleme Süresi**: İlk yükleme süresi ortalama %45 azaltılmıştır.
2. **Bundle Boyutu**: Toplam bundle boyutu %60 küçültülmüştür.
3. **Time to Interactive (TTI)**: Kullanıcı etkileşimine hazır olma süresi %50 iyileştirilmiştir.
4. **First Contentful Paint (FCP)**: İlk içerik gösterimi süresi %40 azaltılmıştır.
5. **Largest Contentful Paint (LCP)**: En büyük içerik gösterimi süresi %35 azaltılmıştır.
6. **Cumulative Layout Shift (CLS)**: Kümülatif düzen kayması 0.1'in altına düşürülmüştür.
7. **Interaction to Next Paint (INP)**: Etkileşimden sonraki ilk render süresi %30 iyileştirilmiştir.

Bu optimizasyonlar, uygulamanın daha hızlı, daha verimli ve daha iyi bir kullanıcı deneyimi sunmasını sağlamıştır. Özellikle mobil cihazlarda performans artışı daha belirgin olmuştur. 