import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCitizenManzelatReviewComponent } from './citizen-manzelat-review.component';

describe('AdminCitizenManzelatReviewComponent', () => {
  let component: AdminCitizenManzelatReviewComponent;
  let fixture: ComponentFixture<AdminCitizenManzelatReviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminCitizenManzelatReviewComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCitizenManzelatReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
