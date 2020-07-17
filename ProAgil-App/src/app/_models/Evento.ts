import { Lote } from './Lote';
import { RedeSocial } from './RedeSocial';
import { Palestrante } from './Palestrante';

export class Evento {

    constructor() {
    }

    id?: number;
    local: string;
    dataEvento: Date;
    tema: string;
    qtdPessoas: number;
    lotes: Lote[];
    imagemUrl: string;
    telefone: string;
    email: string;
    redesSociais: RedeSocial[];
    palestranteEventos: Palestrante[];
}
