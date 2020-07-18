import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { Evento } from 'src/app/_models/Evento';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { EventoService } from 'src/app/_services/evento.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-edit',
  templateUrl: './eventoEdit.component.html',
  styleUrls: ['./eventoEdit.component.scss']
})
export class EventoEditComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private eventoService: EventoService,
    private activatedRouter: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) {
  }

  registerForm: FormGroup;
  evento = new Evento();
  dataEvento: Date;
  datePickerConfig: Partial<BsDatepickerConfig>;
  imagemUrl = 'assets/img/upload.png';
  fileNameToUpdate: string;
  foto: File;

  get lotes(): FormArray {
    // tslint:disable-next-line: no-angle-bracket-type-assertion
    return <FormArray> this.registerForm.get('lotes');
  }

  get redesSociais(): FormArray {
    // tslint:disable-next-line: no-angle-bracket-type-assertion
    return <FormArray> this.registerForm.get('redesSociais');
  }

  ngOnInit() {
    this.criarEventoForm();
    this.carregarEvento();
    this.configurarDatePicker();
  }

  carregarEvento() {
    const idEvento = +this.activatedRouter.snapshot.paramMap.get('id');
    this.eventoService.obterEventoPeloId(idEvento).subscribe(
      (evento: Evento) => {
        this.evento = Object.assign({}, evento);

        this.fileNameToUpdate = evento.imagemUrl.toString();
        this.imagemUrl = `http://localhost:5000/resources/images/${this.evento.imagemUrl}`;
        this.evento.imagemUrl = '';

        this.registerForm.patchValue(this.evento);

        this.evento.lotes.forEach(lote => {
          this.lotes.push(this.criarLoteForm(lote));
        });

        this.evento.redesSociais.forEach(redeSocial => {
          this.redesSociais.push(this.criarRedeSocialForm(redeSocial));
        });
      }
    );
  }

  criarEventoForm() {
    this.registerForm = this.formBuilder.group({
      id: [],
      tema: [ '', [ Validators.required, Validators.minLength(4), Validators.maxLength(50) ]],
      local: [ '', Validators.required ],
      dataEvento: [ '',  Validators.required ],
      qtdPessoas: [ '', [Validators.required, Validators.max(12000)] ],
      imagemUrl: [''],
      telefone: [ '', Validators.required ],
      email: [ '', [Validators.required, Validators.email] ],
      lotes: this.formBuilder.array([]),
      redesSociais: this.formBuilder.array([]),
    });
  }

  ehCampoInvalido(fieldName: string): boolean {
    const field = this.registerForm.get(fieldName);
    return field.errors && field.touched;
  }

  criarLoteForm(lote: any): FormGroup {
    return this.formBuilder.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    });
  }

  criarRedeSocialForm(redeSocial: any) {
    return this.formBuilder.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required]
    });
  }

  configurarDatePicker() {
    this.datePickerConfig = {
      dateInputFormat: 'DD/MM/YYYY HH:mm:ss'
    };
  }

  onFileChange(evento: any, file: FileList) {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemUrl = event.target.result;

    this.foto = evento.target.files;
    reader.readAsDataURL(file[0]);
    this.fileNameToUpdate = file[0].name;
  }

  adicionarLote() {
    this.lotes.push(this.criarLoteForm({ id: 0 }));
  }

  removerLote(id: number) {
    this.lotes.removeAt(id);
  }

  adicionarRedeSocial() {
    this.redesSociais.push(this.criarRedeSocialForm({ id: 0 }));
  }

  removerRedeSocial(id: number) {
    this.redesSociais.removeAt(id);
  }

  salvarEvento() {
    this.evento = Object.assign({}, this.registerForm.value);
    this.evento.imagemUrl = this.fileNameToUpdate;

    this.salvarFoto();

    this.eventoService.editarEvento(this.evento).subscribe(
      () => {
        this.toastr.success('Evento salvo com sucesso!');
        this.imagemUrl = `http://localhost:5000/resources/images/${this.evento.imagemUrl}`;
        this.router.navigate(['/eventos']);
      }, error => {
        console.log(error);
        this.toastr.error('Erro ao salvar evento. Confira o log para mais informações.');
      });
  }

  salvarFoto() {
    if (this.registerForm.get('imagemUrl').value !== '') {
      this.eventoService.salvarFoto(this.foto).subscribe(
        () => {
          this.imagemUrl = `http://localhost:5000/resources/images/${this.evento.imagemUrl}`;
        }
      );
    }
  }
}
