import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RevirComponent } from './revir.component';

describe('RevirComponent', () => {
  let component: RevirComponent;
  let fixture: ComponentFixture<RevirComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RevirComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RevirComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
