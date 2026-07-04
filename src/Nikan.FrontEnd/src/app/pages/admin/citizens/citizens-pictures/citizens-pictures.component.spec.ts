import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCitizensPicturesComponent } from './citizens-pictures.component';

describe('AdminCitizensPicturesComponent', () => {
  let component: AdminCitizensPicturesComponent;
  let fixture: ComponentFixture<AdminCitizensPicturesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminCitizensPicturesComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCitizensPicturesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
