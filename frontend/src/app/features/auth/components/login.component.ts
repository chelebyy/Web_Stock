import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/authentication/auth.service';

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
export class LoginComponent implements OnInit, AfterViewInit {
  @ViewChild('matrixCanvas') matrixCanvas!: ElementRef<HTMLCanvasElement>;
  private ctx!: CanvasRenderingContext2D;
  private mouseX: number = 0;
  private mouseY: number = 0;
  private characters: string = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%^&*()';
  private particles: GridParticle[][] = [];
  private animationFrameId: number = 0;
  private gridSize: number = 15;
  private cols: number = 0;
  private rows: number = 0;
  private mouseRadius: number = 150; // Mouse etki alanı

  sicil: string = '';
  password: string = '';
  rememberMe: boolean = false;
  showPassword: boolean = false;
  toastMessage: string = '';
  toastTimeout: any;
  toastType: 'success' | 'error' = 'success';

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  @HostListener('mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    this.mouseX = event.clientX;
    this.mouseY = event.clientY;
  }

  ngAfterViewInit() {
    this.initializeCanvas();
    this.animate();
  }

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

  private animate() {
    // Arka planı temizle - daha koyu ton
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
          // Mouse yakınındaki karakterlerin alpha değerini güncelle
          const targetAlpha = this.map(distance, 0, this.mouseRadius, 2, 0.05); // 1.5'ten 2'ye çıkardık
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

  // Yardımcı fonksiyonlar
  private map(value: number, start1: number, stop1: number, start2: number, stop2: number): number {
    return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
  }

  private lerp(start: number, end: number, amt: number): number {
    return (1 - amt) * start + amt * end;
  }

  ngOnDestroy() {
    if (this.animationFrameId) {
      cancelAnimationFrame(this.animationFrameId);
    }
    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  showToast(message: string, type: 'success' | 'error' = 'success', duration: number = 3000) {
    this.toastMessage = message;
    this.toastType = type;
    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
    this.toastTimeout = setTimeout(() => {
      this.closeToast();
    }, duration);
  }

  closeToast() {
    this.toastMessage = '';
    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
  }

  onSubmit(): void {
    if (!this.sicil || !this.password) {
      this.showToast('Sicil numarası ve şifre gereklidir!', 'error');
      return;
    }

    console.log('Login isteği gönderiliyor:', { sicil: this.sicil, password: this.password });

    this.authService.login({ sicil: this.sicil, password: this.password }).subscribe({
      next: (response) => {
        console.log('Login başarılı:', response);
        this.showToast('Giriş başarılı!', 'success');
        
        // Kullanıcı rolüne göre yönlendirme
        const user = this.authService.getCurrentUser();
        const targetRoute = user?.isAdmin ? '/dashboard/admin-dashboard' : '/dashboard/user-dashboard';
        console.log('Yönlendiriliyor:', targetRoute);
        
        setTimeout(() => {
          this.router.navigate([targetRoute])
            .then(() => console.log('Yönlendirme başarılı'))
            .catch(err => console.error('Yönlendirme hatası:', err));
        }, 1000);
      },
      error: (error) => {
        console.error('Login hatası:', error);
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

  ngOnInit(): void {
    // Kullanıcı zaten giriş yapmışsa, dashboard'a yönlendir
    if (this.authService.isLoggedIn()) {
      const user = this.authService.getCurrentUser();
      const targetRoute = user?.isAdmin ? '/dashboard/admin-dashboard' : '/dashboard/user-dashboard';
      this.router.navigate([targetRoute]);
    }
  }
}

class GridParticle {
  public alpha: number = 0;
  public color: string;

  constructor(
    public x: number,
    public y: number,
    public char: string
  ) {
    const hue = Math.random() * 360;
    const saturation = 65 + Math.random() * 35; // Biraz daha soft renkler
    const lightness = 55 + Math.random() * 20; // Biraz daha yumuşak parlaklık
    this.color = `hsl(${hue}, ${saturation}%, ${lightness}%)`;
  }
}
