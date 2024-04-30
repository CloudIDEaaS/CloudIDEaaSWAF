import { EditLabelComponent } from './edit-label';

export class EditChangeArgs {
  editLabel: EditLabelComponent;
  editInput: HTMLInputElement;
  allowChange = true;
  oldValue: string;
  value: string;
}
