import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {
  baseUrl = 'http://localhost:5000/api/eventos';

  constructor(private http: HttpClient) {
  }

  obterEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseUrl);
  }

  obterEventoPeloTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseUrl}/getByTema/${tema}`);
  }

  obterEventoPeloId(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseUrl}/${id}`);
  }

  adicionarEvento(evento: Evento) {
    return this.http.post<Evento>(this.baseUrl, evento);
  }

  editarEvento(evento: Evento) {
    return this.http.put<Evento>(`${this.baseUrl}/${evento.id}`, evento);
  }

  excluirEvento(id: number) {
    return this.http.delete<Evento>(`${this.baseUrl}/${id}`);
  }

  salvarFoto(arquivo: File) {
    const foto = arquivo[0] as File;
    const formData = new FormData();
    formData.append('foto', foto, foto.name);

    return this.http.post(`${this.baseUrl}/upload`, formData);
  }
}
