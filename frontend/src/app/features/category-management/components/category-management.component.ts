import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { Textarea } from 'primeng/inputtextarea';
import { TooltipModule } from 'primeng/tooltip';
import { CardModule } from 'primeng/card';
import { RippleModule } from 'primeng/ripple';
import { Router, RouterModule } from '@angular/router';
import { signal } from '@angular/core';
import { CategoryService, CategoryDto } from '../../../services/category.service';
import { DeleteConfirmationDialogComponent } from '../../../features/shared/components/delete-confirmation-dialog/delete-confirmation-dialog.component';

@Component({
  selector: 'app-category-management',
  templateUrl: './category-management.component.html',
  styleUrls: ['./category-management.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    ButtonModule,
    ConfirmDialogModule,
    DialogModule,
    InputTextModule,
    Textarea,
    TableModule,
    ToastModule,
    ToolbarModule,
    TooltipModule,
    CardModule,
    RippleModule,
    DeleteConfirmationDialogComponent
  ],
  providers: [ConfirmationService, MessageService]
})
export class CategoryManagementComponent implements OnInit {
  // Servisler
  private categoryService = inject(CategoryService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);
  private fb = inject(FormBuilder);
  private router = inject(Router);

  // State değişkenleri
  categories = signal<CategoryDto[]>([]);
  searchText = signal<string>('');
  categoryDialog = signal<boolean>(false);
  editMode = signal<boolean>(false);
  deleteDialogVisible = signal<boolean>(false);
  selectedCategory: CategoryDto | null = null;
  categoryToDelete: CategoryDto | null = null;

  // Form
  categoryForm: FormGroup;

  constructor() {
    this.categoryForm = this.fb.group({
      id: [null],
      name: ['', [Validators.required]],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe({
      next: (data) => {
        this.categories.set(data);
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: `Kategoriler yüklenirken bir hata oluştu: ${error.message || 'Bilinmeyen hata'}`,
          life: 5000
        });
      }
    });
  }

  openDialog(category?: CategoryDto): void {
    this.editMode.set(!!category);
    
    if (category) {
      this.categoryForm.patchValue({
        id: category.id,
        name: category.name,
        description: category.description || ''
      });
      this.selectedCategory = category;
    } else {
      this.categoryForm.reset();
      this.selectedCategory = null;
    }
    
    this.categoryDialog.set(true);
  }

  hideDialog(): void {
    this.categoryDialog.set(false);
    this.categoryForm.reset();
  }

  saveCategory(): void {
    if (this.categoryForm.invalid) return;
    
    const formValue = this.categoryForm.value;
    
    if (this.editMode()) {
      // Güncelleme işlemi
      this.categoryService.updateCategory(formValue.id, formValue).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Kategori başarıyla güncellendi',
            life: 3000
          });
          this.hideDialog();
          this.loadCategories();
        },
        error: (error) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: `Kategori güncellenirken bir hata oluştu: ${error.message || 'Bilinmeyen hata'}`,
            life: 5000
          });
        }
      });
    } else {
      // Yeni kategori oluşturma
      this.categoryService.createCategory(formValue).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Yeni kategori başarıyla oluşturuldu',
            life: 3000
          });
          this.hideDialog();
          this.loadCategories();
        },
        error: (error) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: `Kategori oluşturulurken bir hata oluştu: ${error.message || 'Bilinmeyen hata'}`,
            life: 5000
          });
        }
      });
    }
  }

  deleteCategory(category: CategoryDto): void {
    this.categoryToDelete = category;
    this.deleteDialogVisible.set(true);
  }

  confirmDelete(): void {
    if (!this.categoryToDelete) return;
    
    this.categoryService.deleteCategory(this.categoryToDelete.id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: `${this.categoryToDelete?.name} kategorisi başarıyla silindi`,
          life: 3000
        });
        this.categoryToDelete = null;
        this.deleteDialogVisible.set(false);
        this.loadCategories();
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: `Kategori silinirken bir hata oluştu: ${error.message || 'Bilinmeyen hata'}`,
          life: 5000
        });
        this.deleteDialogVisible.set(false);
      }
    });
  }

  cancelDelete(): void {
    this.categoryToDelete = null;
    this.deleteDialogVisible.set(false);
  }

  filterCategories(searchText: string): void {
    this.searchText.set(searchText);
  }

  getFilteredCategories(): CategoryDto[] {
    const search = this.searchText().toLowerCase();
    
    if (!search) {
      return this.categories();
    }
    
    return this.categories().filter(category => {
      return category.name.toLowerCase().includes(search) || 
        (category.description && category.description.toLowerCase().includes(search));
    });
  }

  goBack(): void {
    this.router.navigate(['/dashboard/admin-dashboard']);
  }
} 