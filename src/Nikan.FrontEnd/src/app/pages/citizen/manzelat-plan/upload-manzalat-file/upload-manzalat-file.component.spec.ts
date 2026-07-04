import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenDocumentsComponent } from './documents.component';

describe('CitizenDocumentsComponent', () => {
  let component: CitizenDocumentsComponent;
  let fixture: ComponentFixture<CitizenDocumentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CitizenDocumentsComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
