import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCitizenAdvancedSearchComponent } from './citizen-advanced-search.component';

describe('AdminCitizenAdvancedSearchComponent', () => {
  let component: AdminCitizenAdvancedSearchComponent;
  let fixture: ComponentFixture<AdminCitizenAdvancedSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminCitizenAdvancedSearchComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCitizenAdvancedSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
