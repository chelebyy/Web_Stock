import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BilgiIslemComponent } from './bilgi-islem.component';

describe('BilgiIslemComponent', () => {
  let component: BilgiIslemComponent;
  let fixture: ComponentFixture<BilgiIslemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BilgiIslemComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BilgiIslemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
