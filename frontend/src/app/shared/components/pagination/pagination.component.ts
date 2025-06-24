import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginatorModule } from 'primeng/paginator';
import { PaginatorState } from 'primeng/paginator';

/**
 * Uygulama genelinde tutarlı bir sayfalama deneyimi sağlayan bileşen.
 * PrimeNG Paginator bileşenini sarmalar ve ek özellikler ekler.
 * 
 * Kullanım örneği:
 * ```html
 * <app-pagination
 *   [totalRecords]="100"
 *   [rows]="10"
 *   [page]="0"
 *   (pageChange)="onPageChange($event)"
 * ></app-pagination>
 * ```
 */
@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule, PaginatorModule],
  template: `
    <p-paginator
      [first]="first"
      [rows]="rows"
      [totalRecords]="totalRecords"
      [rowsPerPageOptions]="rowsPerPageOptions"
      [showCurrentPageReport]="showCurrentPageReport"
      [showPageLinks]="showPageLinks"
      [showJumpToPageDropdown]="showJumpToPageDropdown"
      [showFirstLastIcon]="showFirstLastIcon"
      [currentPageReportTemplate]="currentPageReportTemplate"
      [alwaysShow]="alwaysShow"
      [styleClass]="styleClass"
      (onPageChange)="onPageChange($event)"
    ></p-paginator>
  `,
  styles: [`
    :host ::ng-deep {
      .p-paginator {
        padding: 0.75rem;
        border-radius: 0.375rem;
        background-color: #ffffff;
        border: 1px solid #e2e8f0;
        
        .p-paginator-pages .p-paginator-page.p-highlight {
          background-color: #3B82F6;
          color: #ffffff;
        }
        
        .p-paginator-element {
          min-width: 2.25rem;
          height: 2.25rem;
          margin: 0 0.125rem;
          border-radius: 0.25rem;
          
          &:focus {
            box-shadow: 0 0 0 2px #ffffff, 0 0 0 4px #3B82F6;
          }
        }
        
        .p-dropdown {
          height: 2.25rem;
          
          .p-dropdown-label {
            padding-right: 2.5rem;
          }
        }
      }
    }
  `]
})
export class PaginationComponent {
  @Input() first = 0;
  @Input() rows = 10;
  @Input() totalRecords = 0;
  @Input() rowsPerPageOptions: number[] = [10, 25, 50];
  @Input() showCurrentPageReport = true;
  @Input() showPageLinks = true;
  @Input() showJumpToPageDropdown = false;
  @Input() showFirstLastIcon = true;
  @Input() currentPageReportTemplate = '{currentPage} / {totalPages}';
  @Input() alwaysShow = true;
  @Input() styleClass = '';
  
  @Output() pageChange = new EventEmitter<PaginatorState>();
  
  /**
   * Sayfa değişikliği olayını yönetir
   */
  onPageChange(event: PaginatorState): void {
    this.pageChange.emit(event);
  }
  
  /**
   * Belirli bir sayfaya gider
   */
  goToPage(page: number): void {
    if (page >= 0 && page <= this.getPageCount() - 1) {
      this.first = page * this.rows;
      this.onPageChange({
        first: this.first,
        rows: this.rows,
        page: page,
        pageCount: this.getPageCount()
      });
    }
  }
  
  /**
   * Toplam sayfa sayısını hesaplar
   */
  getPageCount(): number {
    return Math.ceil(this.totalRecords / this.rows) || 1;
  }
  
  /**
   * Mevcut sayfa numarasını döndürür
   */
  getCurrentPage(): number {
    return Math.floor(this.first / this.rows);
  }
} 