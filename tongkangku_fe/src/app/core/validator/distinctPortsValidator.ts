import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function distinctPortsValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const loadingPort = control.get('loadingPortId')?.value;
    const dischargingPort = control.get('dischargingPortId')?.value;

    if (!loadingPort || !dischargingPort) return null;

    return loadingPort === dischargingPort ? { samePorts: true } : null;
  };
}
