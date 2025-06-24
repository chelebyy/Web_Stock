import { Component, OnInit, inject, ChangeDetectionStrategy, ChangeDetectorRef, effect } from '@angular/core';
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
import { DropdownModule } from 'primeng/dropdown';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { TooltipModule } from 'primeng/tooltip';
import { CardModule } from 'primeng/card';
import { RippleModule } from 'primeng/ripple';
import { Router, RouterModule } from '@angular/router';
import { ProductService, CreateProductCommand, UpdateProductCommand, ProductDto } from '../../../services/product.service';
import { CategoryService } from '../../../services/category.service';
import { DeleteConfirmationDialogComponent } from '../../../features/shared/components/delete-confirmation-dialog/delete-confirmation-dialog.component';
import { InputNumberModule } from 'primeng/inputnumber';
import { ProductManagementStateService } from '../services/ProductManagementStateService';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-product-management',
  templateUrl: './product-management.component.html',
  styleUrls: ['./product-management.component.scss'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule, FormsModule, ReactiveFormsModule, RouterModule, ButtonModule, ConfirmDialogModule,
    DialogModule, InputTextModule, InputTextareaModule, InputNumberModule, TableModule, ToastModule,
    ToolbarModule, DropdownModule, TooltipModule, CardModule, RippleModule, DeleteConfirmationDialogComponent
  ],
  providers: [ConfirmationService, MessageService, ProductManagementStateService]
})
export class ProductManagementComponent implements OnInit {
  // Servisler ve State
  public state = inject(ProductManagementStateService);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  productForm: FormGroup;

  constructor() {
    this.productForm = this.fb.group({
      id: [null],
      name: ['', [Validators.required]],
      description: [''],
      stockLevel: [0, [Validators.required, Validators.min(0)]],
      categoryId: [null, [Validators.required]]
    });

    // State'teki değişikliklere göre formu güncelle
    effect(() => {
      const product = this.state.selectedProduct();
      if (product) {
        this.productForm.patchValue(product);
      } else {
        this.productForm.reset({ stockLevel: 0 });
      }
    });
  }

  ngOnInit(): void {
    this.loadProducts();
    this.loadCategories();
  }

  async loadProducts(): Promise<void> {
    this.state.setLoading(true);
    try {
      const data = await firstValueFrom(this.productService.getAllProducts());
      this.state.setProducts(data);
    } catch (error: any) {
      this.state.setError(error.message || 'Ürünler yüklenirken bir hata oluştu.');
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: this.state.error()! });
    } finally {
      this.cdr.markForCheck();
    }
  }

  async loadCategories(): Promise<void> {
    try {
      const data = await firstValueFrom(this.categoryService.getAllCategories());
      this.state.setCategories(data);
    } catch (error: any) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: `Kategoriler yüklenirken bir hata oluştu: ${error.message || 'Bilinmeyen hata'}` });
    } finally {
      this.cdr.markForCheck();
    }
  }

  openDialog(product?: ProductDto): void {
    this.state.openProductDialog(product);
  }

  hideDialog(): void {
    this.state.hideProductDialog();
  }

  async saveProduct(): Promise<void> {
    if (this.productForm.invalid) return;

    const formValue = this.productForm.value;
    const command = {
      id: formValue.id,
      name: formValue.name,
      description: formValue.description || '',
      stockLevel: formValue.stockLevel,
      categoryId: formValue.categoryId
    };

    this.state.setLoading(true);
    try {
      if (this.state.isEditMode()) {
        await firstValueFrom(this.productService.updateProduct(command.id, command as UpdateProductCommand));
        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Ürün başarıyla güncellendi' });
      } else {
        await firstValueFrom(this.productService.createProduct(command as CreateProductCommand));
        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Yeni ürün başarıyla oluşturuldu' });
      }
      this.hideDialog();
      await this.loadProducts();
    } catch (error: any) {
      this.state.setError(error.message || 'İşlem sırasında bir hata oluştu.');
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: this.state.error()! });
    } finally {
      this.state.setLoading(false);
      this.cdr.markForCheck();
    }
  }

  deleteProduct(product: ProductDto): void {
    this.confirmationService.confirm({
      message: `${product.name} adlı ürünü silmek istediğinizden emin misiniz?`,
      header: 'Silme Onayı',
      icon: 'pi pi-exclamation-triangle',
      accept: async () => {
        this.state.setLoading(true);
        try {
          await firstValueFrom(this.productService.deleteProduct(product.id));
          this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Ürün başarıyla silindi' });
          await this.loadProducts();
        } catch (error: any) {
          this.state.setError(error.message || 'Silme işlemi sırasında bir hata oluştu.');
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: this.state.error()! });
        } finally {
          this.state.setLoading(false);
          this.cdr.markForCheck();
        }
      }
    });
  }

  filterProducts(event: Event): void {
    const searchText = (event.target as HTMLInputElement).value;
    this.state.setSearchText(searchText);
  }

  filterByCategory(event: any): void {
    const categoryId = event.value;
    this.state.setSelectedCategoryId(categoryId);
  }

  getCategoryName(categoryId: number): string {
    return this.state.categories().find(c => c.id === categoryId)?.name || '';
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
} 