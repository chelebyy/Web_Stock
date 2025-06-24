import { Injectable, signal, computed, WritableSignal } from '@angular/core';
import { ProductDto } from '../../../services/product.service'; // Bu yolu projenize göre ayarlayın
import { CategoryDto } from '../../../services/category.service'; // Bu yolu projenize göre ayarlayın

export interface ProductManagementState {
  products: ProductDto[];
  categories: CategoryDto[];
  searchText: string;
  selectedCategoryId: number | null;
  productDialogVisible: boolean;
  isEditMode: boolean;
  selectedProduct: ProductDto | null;
  loading: boolean;
  error: string | null;
}

const initialState: ProductManagementState = {
  products: [],
  categories: [],
  searchText: '',
  selectedCategoryId: null,
  productDialogVisible: false,
  isEditMode: false,
  selectedProduct: null,
  loading: false,
  error: null,
};

@Injectable({
  providedIn: 'root'
})
export class ProductManagementStateService {
  private state: WritableSignal<ProductManagementState> = signal(initialState);

  // Selectors (Public Signals)
  products = computed(() => this.state().products);
  categories = computed(() => this.state().categories);
  searchText = computed(() => this.state().searchText);
  selectedCategoryId = computed(() => this.state().selectedCategoryId);
  productDialogVisible = computed(() => this.state().productDialogVisible);
  isEditMode = computed(() => this.state().isEditMode);
  selectedProduct = computed(() => this.state().selectedProduct);
  loading = computed(() => this.state().loading);
  error = computed(() => this.state().error);

  filteredProducts = computed(() => {
    const search = this.searchText().toLowerCase();
    const categoryId = this.selectedCategoryId();
    
    return this.products().filter(product => {
      const nameMatch = product.name.toLowerCase().includes(search);
      const descriptionMatch = product.description ? product.description.toLowerCase().includes(search) : false;
      const categoryMatch = !categoryId || product.categoryId === categoryId;
      
      return (nameMatch || descriptionMatch) && categoryMatch;
    });
  });

  // Actions (State Modifiers)
  setProducts(products: ProductDto[]) {
    this.state.update(s => ({ ...s, products, loading: false }));
  }

  setCategories(categories: CategoryDto[]) {
    this.state.update(s => ({ ...s, categories }));
  }

  setSearchText(searchText: string) {
    this.state.update(s => ({ ...s, searchText }));
  }

  setSelectedCategoryId(categoryId: number | null) {
    this.state.update(s => ({ ...s, selectedCategoryId: categoryId }));
  }

  openProductDialog(product?: ProductDto) {
    this.state.update(s => ({
      ...s,
      productDialogVisible: true,
      isEditMode: !!product,
      selectedProduct: product || null
    }));
  }

  hideProductDialog() {
    this.state.update(s => ({
      ...s,
      productDialogVisible: false,
      isEditMode: false,
      selectedProduct: null
    }));
  }
  
  setLoading(loading: boolean) {
    this.state.update(s => ({ ...s, loading }));
  }

  setError(error: string | null) {
    this.state.update(s => ({ ...s, error, loading: false }));
  }
} 