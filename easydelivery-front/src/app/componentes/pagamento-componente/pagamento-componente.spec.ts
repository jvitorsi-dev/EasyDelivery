import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PagamentoComponente } from './pagamento-componente';

describe('PagamentoComponente', () => {
  let component: PagamentoComponente;
  let fixture: ComponentFixture<PagamentoComponente>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PagamentoComponente],
    }).compileComponents();

    fixture = TestBed.createComponent(PagamentoComponente);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
