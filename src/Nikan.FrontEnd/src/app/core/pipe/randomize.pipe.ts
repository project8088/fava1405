import { Pipe, PipeTransform, Injectable } from '@angular/core';

@Pipe({
  name: 'random',
})
@Injectable()
export class ArrayRandomizePipe implements PipeTransform {
  transform(array: any, Randomize: boolean = true): any[] {
    if (!Array.isArray(array)) {
      return;
    }
    if (Randomize) {
      return this.shuffle(array);
    } else return array;
  }

  shuffle(array) {
    var currentIndex = array.length,
      temporaryValue,
      randomIndex;

    // While there remain elements to shuffle...
    while (0 !== currentIndex) {
      // Pick a remaining element...
      randomIndex = Math.floor(Math.random() * currentIndex);
      currentIndex -= 1;

      // And swap it with the current element.
      temporaryValue = array[currentIndex];
      array[currentIndex] = array[randomIndex];
      array[randomIndex] = temporaryValue;
    }

    return array;
  }
}
