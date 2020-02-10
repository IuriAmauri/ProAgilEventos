import { Component, OnInit } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, defineLocale, BsLocaleService, ptBrLocale, BsDatepickerConfig } from 'ngx-bootstrap';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {
  titulo = 'Eventos';
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  modalRef: BsModalRef;
  registerForm: FormGroup;
  datePickerConfig: Partial<BsDatepickerConfig>;

  evento: Evento;
  eventosFiltrados: any = [];
  eventos: any = [];
  FiltroLista: string;
  dataEvento: Date;

  constructor(
    private eventoService: EventoService,
    private formBuilder: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService) {
    this.localeService.use('pt-br');
  }

  get fitroLista(): string {
    return this.FiltroLista;
  }

  set filtroLista(filtro: string) {
    this.FiltroLista = filtro;
    this.eventosFiltrados = this.filtrarEventos(this.FiltroLista);
  }

  ngOnInit() {
    this.criarForm();
    this.obterEventos();
    this.configurarDatePicker();
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  criarForm() {
    this.registerForm = this.formBuilder.group({
      id: [],
      tema: [ '', [ Validators.required, Validators.minLength(4), Validators.maxLength(50) ]],
      local: [ '', Validators.required ],
      dataEvento: [ '',  Validators.required ],
      qtdPessoas: [ '', [Validators.required, Validators.max(12000)] ],
      imagemUrl: [ '', Validators.required ],
      telefone: [ '', Validators.required ],
      email: [ '', [Validators.required, Validators.email] ],
      lotes: [],
      redesSociais: [],
      palestranteEventos: []
    });
  }

  obterEventos() {
    this.eventoService.obterEventos().subscribe(
      (eventosInternal: Evento[]) => {
        this.eventos = eventosInternal;
        this.eventosFiltrados = eventosInternal;
      }, error => {
        console.log(error);
      });
  }

  filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(evento => {
      return evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1;
    });
  }

  ehCampoInvalido(fieldName: string): boolean {
    const field = this.registerForm.get(fieldName);
    return field.errors && field.touched;
  }

  configurarDatePicker() {
    this.datePickerConfig = {
      dateInputFormat: 'DD/MM/YYYY HH:mm'
    };
  }

  adicionarEvento(template: any) {
    this.registerForm.reset();
    template.show();
  }

  editarEvento(template: any, evento: Evento) {
    this.dataEvento = evento.dataEvento;
    this.registerForm.patchValue(evento);
    template.show();
  }

  salvarEvento(template: any) {
    if (!this.registerForm.valid) {
      return;
    }

    this.evento = Object.assign({}, this.registerForm.value);

    if (!this.evento.id) {
      this.eventoService.adicionarEvento(this.evento).subscribe(
        () => {
          template.hide();
          this.obterEventos();
          this.toastr.success('Evento salvo com sucesso!');
        }, error => {
          console.log(error);
          this.toastr.error('Erro ao salvar evento. Confira o log para mais informações.');
        }
        );
      } else {
        this.eventoService.editarEvento(this.evento).subscribe(
          () => {
            template.hide();
            this.obterEventos();
            this.toastr.success('Evento salvo com sucesso!');
          }, error => {
            console.log(error);
            this.toastr.error('Erro ao salvar evento. Confira o log para mais informações.');
          });
      }
  }

  confirmarExclusaoEvento(template: any, evento: Evento) {
    this.evento = evento;
    template.show();
  }

  excluirEvento(template: any, eventoId: number) {
    if (!eventoId) {
      return;
    }

    this.eventoService.excluirEvento(eventoId).subscribe(
      () => {
        this.obterEventos();
        this.toastr.success('Evento excluído!');
      }, error => {
        console.log(error);
        this.toastr.error('Erro ao excluir evento. Confira o log para mais informações.');
      });

    template.hide();
  }
}
