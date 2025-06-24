import { Pipe, PipeTransform } from '@angular/core';

/**
 * Dizileri belirli bir kritere göre filtrelemek için kullanılan pipe.
 * 
 * Kullanım:
 * {{ items | filter:'name':'John' }}        // name özelliği 'John' içeren öğeleri filtrele
 * {{ items | filter:'name':'John':true }}   // name özelliği 'John' ile tam eşleşen öğeleri filtrele
 */
@Pipe({
  name: 'filter',
  standalone: true
})
export class FilterPipe implements PipeTransform {
  /**
   * Diziyi belirtilen kritere göre filtreler.
   * 
   * @param items Filtrelenecek dizi
   * @param field Filtreleme yapılacak alan adı
   * @param value Filtreleme değeri
   * @param exactMatch Tam eşleşme yapılıp yapılmayacağı (varsayılan: false)
   * @returns Filtrelenmiş dizi
   */
  transform<T>(items: T[], field: string, value: any, exactMatch = false): T[] {
    if (!items || !field || value === undefined || value === null) {
      return items;
    }
    
    return items.filter(item => {
      const itemValue = this.getPropertyValue(item, field);
      
      if (itemValue === undefined || itemValue === null) {
        return false;
      }
      
      if (exactMatch) {
        return itemValue === value;
      }
      
      if (typeof itemValue === 'string' && typeof value === 'string') {
        return itemValue.toLowerCase().includes(value.toLowerCase());
      }
      
      return String(itemValue).includes(String(value));
    });
  }
  
  /**
   * Nesnenin belirtilen özelliğinin değerini alır.
   * Nokta notasyonu ile iç içe özelliklere erişim desteklenir (örn: 'user.address.city').
   * 
   * @param obj Nesne
   * @param path Özellik yolu
   * @returns Özellik değeri
   */
  private getPropertyValue(obj: any, path: string): any {
    if (!obj || !path) {
      return undefined;
    }
    
    const properties = path.split('.');
    let value = obj;
    
    for (const prop of properties) {
      if (value === null || value === undefined) {
        return undefined;
      }
      
      value = value[prop];
    }
    
    return value;
  }
} 