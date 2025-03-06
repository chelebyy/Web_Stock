import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-revir',
  standalone: true,
  imports: [CommonModule, CardModule],
  templateUrl: './revir.component.html',
  styleUrl: './revir.component.scss'
})
export class RevirComponent {
  title = 'Revir Sayfası';
}
