import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-bilgi-islem',
  standalone: true,
  imports: [CommonModule, CardModule],
  templateUrl: './bilgi-islem.component.html',
  styleUrl: './bilgi-islem.component.scss'
})
export class BilgiIslemComponent {
  title = signal('Bilgi İşlem Sayfası');
} 