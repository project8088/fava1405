import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenContactComponent } from './contact.component';

describe('CitizenContactComponent', () => {
  let component: CitizenContactComponent;
  let fixture: ComponentFixture<CitizenContactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CitizenContactComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
