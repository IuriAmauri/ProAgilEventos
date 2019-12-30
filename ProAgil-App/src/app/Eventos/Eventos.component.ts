import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-eventos',
  templateUrl: './Eventos.component.html',
  styleUrls: ['./Eventos.component.css']
})
export class EventosComponent implements OnInit {
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  modalRef: BsModalRef;
  registerForm: FormGroup;

  eventosFiltrados: any = [];
  eventos: any = [];
  FiltroLista: string;

  constructor(private eventoService: EventoService, private modalService: BsModalService, private formBuilder: FormBuilder) { }

  get fitroLista(): string {
    return this.FiltroLista;
  }

  set filtroLista(filtro: string) {
    this.FiltroLista = filtro;
    this.eventosFiltrados = this.filtrarEventos(this.FiltroLista);
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { animated: false });
  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  validation() {
    this.registerForm = this.formBuilder.group({
      tema: [ '', [ Validators.required, Validators.minLength(4), Validators.maxLength(50) ]],
      local: [ '', Validators.required ],
      dataEvento: [ '',  Validators.required ],
      qtdPessoas: [ '', [Validators.required, Validators.max(12000)] ],
      imagemUrl: [ '', Validators.required ],
      telefone: [ '', Validators.required ],
      email: [ '', [Validators.required, Validators.email] ],
    });
  }

  salvarAlteracao() {

  }

  getEventos() {
    this.eventoService.getEventos().subscribe(
      (eventosInternal: Evento[]) => {
      this.eventos = eventosInternal;
      this.eventosFiltrados = eventosInternal;
    }, error => {
      console.log(error);
    });
  }

  filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1);
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.registerForm.get(fieldName);

    return field.errors && field.touched;
  }
}
