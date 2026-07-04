import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenCardListComponent } from './card-list.component';

describe('CitizenCardListComponent', () => {
  let component: CitizenCardListComponent;
  let fixture: ComponentFixture<CitizenCardListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CitizenCardListComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenCardListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
