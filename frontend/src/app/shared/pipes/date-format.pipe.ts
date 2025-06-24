import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

/**
 * Tarihleri formatlamak için kullanılan pipe.
 * 
 * Kullanım:
 * {{ date | dateFormat }}             // Varsayılan format: 'dd.MM.yyyy'
 * {{ date | dateFormat:'yyyy-MM-dd' }} // Özel format
 */
@Pipe({
  name: 'dateFormat',
  standalone: true
})
export class DateFormatPipe implements PipeTransform {
  private datePipe = new DatePipe('tr-TR');

  /**
   * Tarihi belirtilen formatta dönüştürür.
   * 
   * @param value Formatlanacak tarih
   * @param format Tarih formatı (varsayılan: 'dd.MM.yyyy')
   * @returns Formatlanmış tarih string'i
   */
  transform(value: any, format = 'dd.MM.yyyy'): string | null {
    if (!value) {
      return null;
    }
    
    return this.datePipe.transform(value, format);
  }
} 