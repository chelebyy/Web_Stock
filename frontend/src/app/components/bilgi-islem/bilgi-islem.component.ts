import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';

@Component({
    selector: 'app-bilgi-islem',
    imports: [CommonModule, CardModule],
    templateUrl: './bilgi-islem.component.html',
    styleUrl: './bilgi-islem.component.scss'
})
export class BilgiIslemComponent {
  title = 'Bilgi İşlem Sayfası';
}
