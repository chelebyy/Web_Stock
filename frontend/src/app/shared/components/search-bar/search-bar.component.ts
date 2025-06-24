import { Component, EventEmitter, Input, Output, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormControl } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';

export interface SearchFilter {
  field: string;
  label: string;
}

/**
 * Uygulama genelinde tutarlı bir arama deneyimi sağlayan bileşen.
 * 
 * Kullanım örneği:
 * ```html
 * <app-search-bar
 *   placeholder="Kullanıcı ara..."
 *   [filters]="filters"
 *   (search)="onSearch($event)"
 * ></app-search-bar>
 * ```
 */
@Component({
  selector: 'app-search-bar',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    DropdownModule
  ],
  template: `
    <div class="search-bar-container" [ngClass]="{'search-bar-expanded': expanded}">
      <div class="search-bar">
        <span class="p-input-icon-left p-input-icon-right">
          <i class="pi pi-search"></i>
          <input
            #searchInput
            type="text"
            pInputText
            [placeholder]="placeholder"
            [formControl]="searchControl"
            (focus)="onFocus()"
            (blur)="onBlur()"
            (keydown.escape)="onEscapeKey()"
            (keydown.enter)="triggerSearch()"
          />
          <i 
            *ngIf="searchControl.value" 
            class="pi pi-times clear-icon" 
            (click)="clearSearch()"
          ></i>
        </span>
        
        <button 
          *ngIf="showSearchButton" 
          pButton 
          type="button" 
          icon="pi pi-search" 
          class="p-button-rounded p-button-text"
          (click)="triggerSearch()"
        ></button>
      </div>
      
      <div class="search-filters" *ngIf="showFilters && filters && filters.length > 0">
        <p-dropdown
          [options]="filters"
          [(ngModel)]="selectedFilter"
          optionLabel="label"
          placeholder="Filtrele"
          [showClear]="true"
          (onChange)="onFilterChange()"
        ></p-dropdown>
      </div>
    </div>
  `,
  styles: [`
    .search-bar-container {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      transition: all 0.3s ease;
    }
    
    .search-bar {
      display: flex;
      align-items: center;
      flex: 1;
      
      .p-inputtext {
        width: 100%;
        transition: all 0.3s ease;
        border-radius: 2rem;
        padding-left: 2.5rem;
        
        &:focus {
          box-shadow: 0 0 0 2px #ffffff, 0 0 0 4px #3B82F6;
        }
      }
      
      .clear-icon {
        cursor: pointer;
        opacity: 0.6;
        
        &:hover {
          opacity: 1;
        }
      }
    }
    
    .search-filters {
      min-width: 150px;
    }
    
    .search-bar-expanded {
      .p-inputtext {
        width: 100%;
      }
    }
    
    :host ::ng-deep {
      .p-dropdown {
        width: 100%;
        
        .p-dropdown-label {
          padding-right: 2.5rem;
        }
      }
    }
  `]
})
export class SearchBarComponent implements OnInit, OnDestroy {
  @Input() placeholder = 'Ara...';
  @Input() debounceTime = 300;
  @Input() minLength = 2;
  @Input() showSearchButton = false;
  @Input() showFilters = false;
  @Input() filters: SearchFilter[] = [];
  @Input() autoSearch = true;
  
  @Output() search = new EventEmitter<{ term: string, filter?: SearchFilter }>();
  @Output() clear = new EventEmitter<void>();
  
  searchControl = new FormControl('');
  selectedFilter: SearchFilter | null = null;
  expanded = false;
  
  private destroy$ = new Subject<void>();
  
  ngOnInit() {
    if (this.autoSearch) {
      this.searchControl.valueChanges.pipe(
        debounceTime(this.debounceTime),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      ).subscribe(value => {
        if (!value || value.length >= this.minLength) {
          this.emitSearch(value || '');
        }
      });
    }
  }
  
  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
  /**
   * Arama olayını tetikler
   */
  triggerSearch() {
    const value = this.searchControl.value || '';
    this.emitSearch(value);
  }
  
  /**
   * Arama olayını yayınlar
   */
  emitSearch(term: string) {
    this.search.emit({
      term,
      filter: this.selectedFilter || undefined
    });
  }
  
  /**
   * Aramayı temizler
   */
  clearSearch() {
    this.searchControl.setValue('');
    this.clear.emit();
  }
  
  /**
   * Filtre değiştiğinde çağrılır
   */
  onFilterChange() {
    if (this.searchControl.value) {
      this.emitSearch(this.searchControl.value);
    }
  }
  
  /**
   * Arama kutusuna odaklanıldığında çağrılır
   */
  onFocus() {
    this.expanded = true;
  }
  
  /**
   * Arama kutusundan çıkıldığında çağrılır
   */
  onBlur() {
    if (!this.searchControl.value) {
      this.expanded = false;
    }
  }
  
  /**
   * Escape tuşuna basıldığında çağrılır
   */
  onEscapeKey() {
    // Sadece arama değeri varsa temizle
    if (this.searchControl.value) {
      this.clearSearch();
    }
  }
} 