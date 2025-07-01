import { Component, ChangeDetectionStrategy, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { trigger, state, style, transition, animate } from '@angular/animations';

// `user-page-permissions.component` dosyasından taşınan arayüzler
interface Permission {
  id: number;
  name: string;
  description: string;
  group: string;
  isGranted: boolean;
  resourceName?: string;
  resourceType?: string;
  action?: string;
}

export interface PagePermission {
  pageName: string;
  pageDescription: string;
  pageIcon: string;
  permissions: Permission[];
  expanded: boolean;
}

@Component({
  selector: 'app-permission-card',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './permission-card.component.html',
  styleUrls: ['./permission-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    trigger('contentExpansion', [
      state('collapsed', style({
        height: '0px',
        opacity: 0,
        overflow: 'hidden',
        paddingTop: '0',
        paddingBottom: '0',
      })),
      state('expanded', style({
        height: '*',
        opacity: 1,
        overflow: 'visible',
      })),
      transition('expanded <=> collapsed', [
        animate('300ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ]),
    ]),
  ],
})
export class PermissionCardComponent {
  @Input() page!: PagePermission;
  @Input() index!: number;
  @Input() anyCardExpanded = false;

  @Output() toggleCard = new EventEmitter<number>();
  @Output() closeCard = new EventEmitter<number>();
  @Output() permissionChanged = new EventEmitter<void>();

  onToggleCard(): void {
    this.toggleCard.emit(this.index);
  }

  onCloseCard(event: MouseEvent): void {
    event.stopPropagation();
    this.closeCard.emit(this.index);
  }

  onCheckboxChange(): void {
    this.permissionChanged.emit();
  }
} 