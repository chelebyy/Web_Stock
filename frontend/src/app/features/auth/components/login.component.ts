import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, HostListener, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/authentication/auth.service';

/**
 * Kullanıcı giriş ekranı bileşeni
 * Animasyonlu bir arka plan ile birlikte giriş formunu gösterir
 */
@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    imports: [
        CommonModule,
        FormsModule
    ],
    standalone: true
})
export class LoginComponent implements OnInit, AfterViewInit, OnDestroy {
  /** Matris animasyon canvasına referans */
  @ViewChild('matrixCanvas') matrixCanvas!: ElementRef<HTMLCanvasElement>;
  
  /** Canvas 2D render context */
  private ctx!: CanvasRenderingContext2D;
  
  /** Fare X pozisyonu */
  private mouseX = 0;
  
  /** Fare Y pozisyonu */
  private mouseY = 0;
  
  /** Matris animasyonunda kullanılacak karakterler */
  private characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%^&*()';
  
  /** Animasyon parçacıkları grid matrisi */
  private particles: GridParticle[][] = [];
  
  /** Animasyon frame ID'si (cancelAnimation için) */
  private animationFrameId = 0;
  
  /** Grid hücre boyutu */
  private gridSize = 15;
  
  /** Grid sütun sayısı */
  private cols = 0;
  
  /** Grid satır sayısı */
  private rows = 0;
  
  /** Fare etki alanı yarıçapı */
  private mouseRadius = 150;

  /** Kullanıcı sicil numarası */
  sicil = '';
  
  /** Kullanıcı şifresi */
  password = '';
  
  /** Beni hatırla seçeneği */
  rememberMe = false;
  
  /** Şifre görünürlük durumu */
  showPassword = false;
  
  /** Toast mesajı */
  toastMessage = '';
  
  /** Toast zamanlayıcısı */
  toastTimeout: any;
  
  /** Toast tipi (success/error) */
  toastType: 'success' | 'error' = 'success';

  /**
   * Login bileşeni constructor
   * @param router Sayfa yönlendirme servisi
   * @param authService Kimlik doğrulama servisi
   */
  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  /**
   * Fare hareket olayını dinler
   * @param event Fare olay nesnesi
   */
  @HostListener('mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    this.mouseX = event.clientX;
    this.mouseY = event.clientY;
  }

  /**
   * View oluşturulduktan sonra tetiklenir
   * Canvas ve animasyonu başlatır
   */
  ngAfterViewInit() {
    this.initializeCanvas();
    this.animate();
  }

  /**
   * Canvas'ı başlatır ve boyutlandırır
   */
  private initializeCanvas() {
    const canvas = this.matrixCanvas.nativeElement;
    this.ctx = canvas.getContext('2d')!;
    
    const updateCanvasSize = () => {
      canvas.width = window.innerWidth;
      canvas.height = window.innerHeight;
      this.setupGrid();
    };

    window.addEventListener('resize', updateCanvasSize);
    updateCanvasSize();
  }

  /**
   * Animasyon için grid sistemini oluşturur
   */
  private setupGrid() {
    this.cols = Math.floor(this.ctx.canvas.width / this.gridSize);
    this.rows = Math.floor(this.ctx.canvas.height / this.gridSize);
    this.particles = [];

    // Grid oluştur
    for (let i = 0; i < this.cols; i++) {
      this.particles[i] = [];
      for (let j = 0; j < this.rows; j++) {
        const x = i * this.gridSize + this.gridSize / 2;
        const y = j * this.gridSize + this.gridSize / 2;
        this.particles[i][j] = new GridParticle(x, y, this.characters[Math.floor(Math.random() * this.characters.length)]);
      }
    }
  }

  /**
   * Animasyon döngüsü
   */
  private animate() {
    // Arka planı temizle - koyu ton
    this.ctx.fillStyle = 'rgba(10, 14, 25, 0.98)';
    this.ctx.fillRect(0, 0, this.ctx.canvas.width, this.ctx.canvas.height);

    // Grid üzerindeki her karakteri güncelle ve çiz
    for (let i = 0; i < this.cols; i++) {
      for (let j = 0; j < this.rows; j++) {
        const particle = this.particles[i][j];
        const distance = Math.sqrt(
          Math.pow(this.mouseX - particle.x, 2) + 
          Math.pow(this.mouseY - particle.y, 2)
        );

        if (distance < this.mouseRadius) {
          // Fare yakınındaki karakterlerin alpha değerini güncelle
          const targetAlpha = this.map(distance, 0, this.mouseRadius, 2, 0.05);
          particle.alpha = this.lerp(particle.alpha, targetAlpha, 0.15);

          // Karakteri rastgele değiştir
          if (Math.random() < 0.05) {
            particle.char = this.characters[Math.floor(Math.random() * this.characters.length)];
          }
        } else {
          // Uzaktaki karakterler için daha düşük alpha değeri
          particle.alpha = this.lerp(particle.alpha, 0.05, 0.1);
        }

        // Karakteri çiz
        if (particle.alpha > 0.01) {
          this.ctx.save();
          this.ctx.globalAlpha = Math.min(particle.alpha, 1);
          this.ctx.fillStyle = particle.color;
          this.ctx.font = '13px monospace';
          this.ctx.fillText(particle.char, particle.x, particle.y);
          this.ctx.restore();
        }
      }
    }

    this.animationFrameId = requestAnimationFrame(() => this.animate());
  }

  /**
   * Değer aralığı dönüştürme yardımcı fonksiyonu
   */
  private map(value: number, start1: number, stop1: number, start2: number, stop2: number): number {
    return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
  }

  /**
   * Doğrusal interpolasyon yardımcı fonksiyonu
   */
  private lerp(start: number, end: number, amt: number): number {
    return (1 - amt) * start + amt * end;
  }

  /**
   * Bileşen yok edildiğinde animasyonu ve zamanlayıcıları temizler
   */
  ngOnDestroy() {
    if (this.animationFrameId) {
      cancelAnimationFrame(this.animationFrameId);
    }
    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
  }

  /**
   * Şifre görünürlüğünü değiştirir
   */
  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  /**
   * Toast mesajı gösterir
   * @param message Gösterilecek mesaj
   * @param type Mesaj tipi (success/error)
   * @param duration Görünme süresi (ms)
   */
  showToast(message: string, type: 'success' | 'error' = 'success', duration = 3000) {
    this.toastMessage = message;
    this.toastType = type;
    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
    this.toastTimeout = setTimeout(() => {
      this.closeToast();
    }, duration);
  }

  /**
   * Toast mesajını kapatır
   */
  closeToast() {
    this.toastMessage = '';
    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
  }

  /**
   * Giriş formunu gönderir
   */
  onSubmit(): void {
    if (!this.sicil || !this.password) {
      this.showToast('Sicil numarası ve şifre gereklidir!', 'error');
      return;
    }

    this.authService.login({ sicil: this.sicil, password: this.password }).subscribe({
      next: (response) => {
        this.showToast('Giriş başarılı!', 'success');
        
        // Kullanıcı rolüne göre yönlendirme
        const user = this.authService.getCurrentUser();
        const targetRoute = user?.isAdmin ? '/dashboard/admin-dashboard' : '/dashboard/user-dashboard';
        
        setTimeout(() => {
          this.router.navigate([targetRoute])
            .then(() => console.log('Yönlendirme başarılı'))
            .catch(err => console.error('Yönlendirme hatası:', err));
        }, 1000);
      },
      error: (error) => {
        let errorMessage = 'Bir hata oluştu!';
        
        if (error.status === 400) {
          errorMessage = error.error?.message || 'Geçersiz sicil numarası veya şifre!';
        } else if (error.status === 401) {
          errorMessage = 'Sicil numarası veya şifre hatalı!';
        }
        
        this.showToast(errorMessage, 'error');
      }
    });
  }

  /**
   * Component başlatıldığında çağrılır
   */
  ngOnInit(): void {
    // Daha önce giriş yapmış kullanıcıyı kontrol et
    if (this.authService.isLoggedIn()) {
      const user = this.authService.getCurrentUser();
      const targetRoute = user?.isAdmin ? '/dashboard/admin-dashboard' : '/dashboard/user-dashboard';
      this.router.navigate([targetRoute]);
    }
  }
}

/**
 * Grid parçacığı sınıfı
 * Matris animasyonundaki her bir karakteri temsil eder
 */
class GridParticle {
  /** Karakter opaklığı */
  public alpha = 0;
  
  /** Karakter rengi */
  public color: string;

  /**
   * Grid parçacığı constructor
   * @param x X koordinatı
   * @param y Y koordinatı
   * @param char Görüntülenecek karakter
   */
  constructor(
    public x: number,
    public y: number,
    public char: string
  ) {
    // Renkli Matrix efekti için farklı renkler
    const colorChoices = [
      [0, Math.floor(Math.random() * 150) + 105, 50],      // Yeşil
      [Math.floor(Math.random() * 150) + 105, 0, 50],      // Kırmızı
      [0, 50, Math.floor(Math.random() * 150) + 105],      // Mavi
      [Math.floor(Math.random() * 100) + 105, 0, 155],     // Mor
      [0, Math.floor(Math.random() * 100) + 155, 155],     // Turkuaz
      [Math.floor(Math.random() * 100) + 155, 155, 0]      // Sarı/Altın
    ];
    
    // Rastgele bir renk seç
    const randomColor = colorChoices[Math.floor(Math.random() * colorChoices.length)];
    this.color = `rgb(${randomColor[0]}, ${randomColor[1]}, ${randomColor[2]})`;
  }
}
