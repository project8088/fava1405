import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenCardComponent } from './citizen-card.component';

describe('CitizenCardComponent', () => {
  let component: CitizenCardComponent;
  let fixture: ComponentFixture<CitizenCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CitizenCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
