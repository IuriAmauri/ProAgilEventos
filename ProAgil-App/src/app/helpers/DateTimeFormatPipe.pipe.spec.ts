import { DateTimeFormatPipePipe } from './DateTimeFormatPipe.pipe';

describe('Pipe: DateTimeFormatPipee', () => {
  it('create an instance', () => {
    const pipe = new DateTimeFormatPipePipe('pt-br');
    expect(pipe).toBeTruthy();
  });
});
