import { Pipe, PipeTransform } from '@angular/core';

/**
 * Uzun metinleri belirli bir uzunlukta kısaltmak için kullanılan pipe.
 * 
 * Kullanım:
 * {{ text | truncate }}             // Varsayılan uzunluk: 50, suffix: '...'
 * {{ text | truncate:20 }}          // Uzunluk: 20, suffix: '...'
 * {{ text | truncate:20:'▶' }}      // Uzunluk: 20, suffix: '▶'
 */
@Pipe({
  name: 'truncate',
  standalone: true
})
export class TruncatePipe implements PipeTransform {
  /**
   * Metni belirtilen uzunlukta kısaltır.
   * 
   * @param value Kısaltılacak metin
   * @param limit Maksimum karakter sayısı (varsayılan: 50)
   * @param suffix Kısaltma sonunda gösterilecek metin (varsayılan: '...')
   * @returns Kısaltılmış metin
   */
  transform(value: string, limit = 50, suffix = '...'): string {
    if (!value) {
      return '';
    }
    
    if (value.length <= limit) {
      return value;
    }
    
    return value.substring(0, limit).trim() + suffix;
  }
} 