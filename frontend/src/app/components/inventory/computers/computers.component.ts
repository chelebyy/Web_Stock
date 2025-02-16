import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-computers',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="computers-container">
      <!-- Burası daha sonra doldurulacak -->
    </div>
  `,
  styles: [`
    .computers-container {
      padding: 2rem;
    }
  `]
})
export class ComputersComponent {
  constructor() {}
} 