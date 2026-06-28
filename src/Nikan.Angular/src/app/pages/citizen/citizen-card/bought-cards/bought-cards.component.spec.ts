import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoughtCardsComponent } from './bought-cards.component';

describe('BoughtCardsComponent', () => {
  let component: BoughtCardsComponent;
  let fixture: ComponentFixture<BoughtCardsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BoughtCardsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BoughtCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
