import { Component, EventEmitter, Input, Output, TemplateRef, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { FormsModule } from '@angular/forms';

export interface Column {
  field: string;
  header: string;
  sortable?: boolean;
  filterable?: boolean;
  hidden?: boolean;
  width?: string;
  styleClass?: string;
}

export interface TableAction {
  label: string;
  icon: string;
  class?: string;
  action: (item: any) => void;
  visible?: (item: any) => boolean;
  disabled?: (item: any) => boolean;
}

/**
 * Uygulama genelinde tutarlı bir veri tablosu deneyimi sağlayan bileşen.
 * PrimeNG Table bileşenini sarmalar ve ek özellikler ekler.
 * 
 * Kullanım örneği:
 * ```html
 * <app-data-table
 *   [data]="users"
 *   [columns]="columns"
 *   [loading]="loading"
 *   [paginator]="true"
 *   [rows]="10"
 *   [totalRecords]="totalRecords"
 *   [globalFilterFields]="['name', 'email']"
 *   (pageChange)="onPageChange($event)"
 *   (sortChange)="onSortChange($event)"
 *   (rowSelect)="onRowSelect($event)"
 * ></app-data-table>
 * ```
 */
@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [
    CommonModule, 
    TableModule, 
    ButtonModule, 
    InputTextModule, 
    DropdownModule, 
    MultiSelectModule,
    FormsModule
  ],
  template: `
    <div class="data-table-container">
      <div class="data-table-header" *ngIf="showHeader">
        <div class="data-table-title" *ngIf="title">
          <h3>{{ title }}</h3>
        </div>
        
        <div class="data-table-actions">
          <div class="data-table-search" *ngIf="showSearch">
            <span class="p-input-icon-left">
              <i class="pi pi-search"></i>
              <input 
                pInputText 
                type="text" 
                placeholder="Ara..." 
                (input)="onGlobalFilter($event)"
              />
            </span>
          </div>
          
          <div class="data-table-column-selector" *ngIf="showColumnSelector">
            <p-multiSelect 
              [options]="availableColumns" 
              [(ngModel)]="selectedColumns" 
              optionLabel="header" 
              selectedItemsLabel="{0} kolon seçildi"
              placeholder="Kolonları seç"
              (onChange)="onColumnSelectionChange()"
            ></p-multiSelect>
          </div>
          
          <div class="data-table-custom-actions">
            <ng-content select="[table-actions]"></ng-content>
          </div>
        </div>
      </div>
      
      <p-table
        #dt
        [value]="data"
        [columns]="displayedColumns"
        [rows]="rows"
        [totalRecords]="totalRecords"
        [paginator]="paginator"
        [lazy]="lazy"
        [loading]="loading"
        [rowHover]="true"
        [showCurrentPageReport]="showCurrentPageReport"
        [rowsPerPageOptions]="rowsPerPageOptions"
        [globalFilterFields]="globalFilterFields"
        [sortField]="sortField"
        [sortOrder]="sortOrder"
        [selectionMode]="selectionMode"
        [(selection)]="selectedItems"
        [scrollable]="scrollable"
        [scrollHeight]="scrollHeight"
        [resizableColumns]="resizableColumns"
        [reorderableColumns]="reorderableColumns"
        [responsive]="responsive"
        [styleClass]="tableStyleClass"
        (onLazyLoad)="onLazyLoad($event)"
        (onRowSelect)="onRowSelect($event)"
        (onRowUnselect)="onRowUnselect($event)"
        (onPage)="onPage($event)"
        (onSort)="onSort($event)"
      >
        <ng-template pTemplate="header">
          <tr>
            <th *ngIf="selectionMode === 'checkbox'" style="width: 3rem">
              <p-tableHeaderCheckbox></p-tableHeaderCheckbox>
            </th>
            <th 
              *ngFor="let col of displayedColumns" 
              [pSortableColumn]="col.sortable ? col.field : undefined"
              [style.width]="col.width"
              [ngClass]="col.styleClass"
            >
              {{ col.header }}
              <p-sortIcon *ngIf="col.sortable" [field]="col.field"></p-sortIcon>
            </th>
            <th *ngIf="actions && actions.length > 0" style="width: 8rem">İşlemler</th>
          </tr>
        </ng-template>
        
        <ng-template pTemplate="body" let-item>
          <tr [pSelectableRow]="item">
            <td *ngIf="selectionMode === 'checkbox'">
              <p-tableCheckbox [value]="item"></p-tableCheckbox>
            </td>
            <td *ngFor="let col of displayedColumns">
              <ng-container *ngIf="!templates || !templates[col.field]; else customTemplate">
                {{ getFieldValue(item, col.field) }}
              </ng-container>
              <ng-template #customTemplate>
                <ng-container 
                  *ngTemplateOutlet="templates[col.field]; context: { $implicit: item, field: col.field }"
                ></ng-container>
              </ng-template>
            </td>
            <td *ngIf="actions && actions.length > 0" class="action-buttons">
              <button 
                *ngFor="let action of actions"
                pButton 
                [icon]="action.icon"
                [label]="action.label"
                [ngClass]="action.class || 'p-button-text p-button-sm'"
                [disabled]="action.disabled ? action.disabled(item) : false"
                (click)="onActionClick(action, item)"
                [style.display]="action.visible ? (action.visible(item) ? 'inline-flex' : 'none') : 'inline-flex'"
              ></button>
            </td>
          </tr>
        </ng-template>
        
        <ng-template pTemplate="emptymessage">
          <tr>
            <td [attr.colspan]="displayedColumns.length + (selectionMode === 'checkbox' ? 1 : 0) + (actions && actions.length > 0 ? 1 : 0)">
              <div class="empty-message">
                <i class="pi pi-info-circle"></i>
                <span>{{ emptyMessage }}</span>
              </div>
            </td>
          </tr>
        </ng-template>
        
        <ng-template pTemplate="loadingbody">
          <tr>
            <td [attr.colspan]="displayedColumns.length + (selectionMode === 'checkbox' ? 1 : 0) + (actions && actions.length > 0 ? 1 : 0)">
              <div class="loading-message">
                <i class="pi pi-spin pi-spinner"></i>
                <span>Yükleniyor...</span>
              </div>
            </td>
          </tr>
        </ng-template>
      </p-table>
    </div>
  `,
  styles: [`
    .data-table-container {
      background-color: #ffffff;
      border-radius: 0.5rem;
      box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06);
      overflow: hidden;
    }
    
    .data-table-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 1rem 1.5rem;
      border-bottom: 1px solid #e2e8f0;
    }
    
    .data-table-title h3 {
      margin: 0;
      font-size: 1.25rem;
      font-weight: 600;
      color: #1e293b;
    }
    
    .data-table-actions {
      display: flex;
      align-items: center;
      gap: 1rem;
    }
    
    .empty-message, .loading-message {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 2rem;
      color: #64748b;
      
      i {
        font-size: 2rem;
        margin-bottom: 0.5rem;
      }
    }
    
    .action-buttons {
      display: flex;
      gap: 0.25rem;
    }
    
    :host ::ng-deep {
      .p-datatable {
        .p-datatable-header {
          background-color: #ffffff;
          border: none;
          padding: 0;
        }
        
        .p-datatable-thead > tr > th {
          background-color: #f8fafc;
          color: #334155;
          font-weight: 600;
          padding: 0.75rem 1rem;
          border-color: #e2e8f0;
        }
        
        .p-datatable-tbody > tr {
          background-color: #ffffff;
          transition: background-color 0.2s;
          
          &:hover {
            background-color: #f1f5f9;
          }
          
          > td {
            padding: 0.75rem 1rem;
            border-color: #e2e8f0;
          }
          
          &.p-highlight {
            background-color: #eff6ff;
            color: #1e40af;
          }
        }
        
        .p-paginator {
          padding: 0.75rem;
          border-top: 1px solid #e2e8f0;
          background-color: #ffffff;
        }
      }
    }
  `]
})
export class DataTableComponent implements OnInit, OnChanges {
  @Input() data: any[] = [];
  @Input() columns: Column[] = [];
  @Input() loading = false;
  @Input() paginator = true;
  @Input() rows = 10;
  @Input() totalRecords = 0;
  @Input() rowsPerPageOptions: number[] = [10, 25, 50];
  @Input() showCurrentPageReport = true;
  @Input() lazy = false;
  @Input() sortField = '';
  @Input() sortOrder = 1;
  @Input() globalFilterFields: string[] = [];
  @Input() selectionMode: 'single' | 'multiple' | 'checkbox' | null = null;
  @Input() scrollable = false;
  @Input() scrollHeight = '';
  @Input() resizableColumns = false;
  @Input() reorderableColumns = false;
  @Input() responsive = true;
  @Input() tableStyleClass = '';
  @Input() title = '';
  @Input() showHeader = true;
  @Input() showSearch = true;
  @Input() showColumnSelector = false;
  @Input() emptyMessage = 'Gösterilecek veri bulunamadı.';
  @Input() actions: TableAction[] = [];
  @Input() templates: Record<string, TemplateRef<any>> = {};
  @Input() selectedItems: any[] = [];
  
  @Output() pageChange = new EventEmitter<any>();
  @Output() sortChange = new EventEmitter<any>();
  @Output() filterChange = new EventEmitter<any>();
  @Output() lazyLoad = new EventEmitter<any>();
  @Output() rowSelect = new EventEmitter<any>();
  @Output() rowUnselect = new EventEmitter<any>();
  @Output() selectionChange = new EventEmitter<any[]>();
  
  selectedColumns: Column[] = [];
  displayedColumns: Column[] = [];
  availableColumns: Column[] = [];
  
  ngOnInit() {
    this.initializeColumns();
  }
  
  ngOnChanges() {
    this.initializeColumns();
  }
  
  /**
   * Kolonları başlatır
   */
  initializeColumns() {
    this.availableColumns = [...this.columns];
    this.selectedColumns = this.columns.filter(col => !col.hidden);
    this.updateDisplayedColumns();
  }
  
  /**
   * Görüntülenen kolonları günceller
   */
  updateDisplayedColumns() {
    if (this.showColumnSelector) {
      this.displayedColumns = [...this.selectedColumns];
    } else {
      this.displayedColumns = this.columns.filter(col => !col.hidden);
    }
  }
  
  /**
   * Kolon seçimi değiştiğinde çağrılır
   */
  onColumnSelectionChange() {
    this.updateDisplayedColumns();
  }
  
  /**
   * Global filtre değiştiğinde çağrılır
   */
  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.filterChange.emit({ value });
  }
  
  /**
   * Lazy load olayını yönetir
   */
  onLazyLoad(event: any) {
    this.lazyLoad.emit(event);
  }
  
  /**
   * Sayfa değişikliği olayını yönetir
   */
  onPage(event: any) {
    this.pageChange.emit(event);
  }
  
  /**
   * Sıralama değişikliği olayını yönetir
   */
  onSort(event: any) {
    this.sortChange.emit(event);
  }
  
  /**
   * Satır seçimi olayını yönetir
   */
  onRowSelect(event: any) {
    this.rowSelect.emit(event);
    this.selectionChange.emit(this.selectedItems);
  }
  
  /**
   * Satır seçimi kaldırma olayını yönetir
   */
  onRowUnselect(event: any) {
    this.rowUnselect.emit(event);
    this.selectionChange.emit(this.selectedItems);
  }
  
  /**
   * İşlem düğmesine tıklandığında çağrılır
   */
  onActionClick(action: TableAction, item: any) {
    action.action(item);
  }
  
  /**
   * Nesnenin belirli bir alanının değerini döndürür
   * Nokta notasyonu ile iç içe alanları destekler (örn: "user.address.city")
   */
  getFieldValue(item: any, field: string): any {
    if (!item || !field) {
      return '';
    }
    
    const fields = field.split('.');
    let value = item;
    
    for (const f of fields) {
      if (value === null || value === undefined) {
        return '';
      }
      value = value[f];
    }
    
    // Son değer undefined ise boş string döndür
    return value === undefined ? '' : value;
  }
} 