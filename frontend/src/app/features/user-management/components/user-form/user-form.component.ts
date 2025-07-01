import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { CheckboxModule } from 'primeng/checkbox';
import { PasswordModule } from 'primeng/password';
import { RippleModule } from 'primeng/ripple';

import { User } from '../../../../shared/models/user.model';
import { Role } from '../../../../shared/models/role.model';
import { RoleService } from '../../../../services/role.service';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    CheckboxModule,
    PasswordModule,
    RippleModule
  ],
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit, OnChanges {
  private readonly fb = inject(FormBuilder);
  private readonly roleService = inject(RoleService);

  @Input() user: User | null = null;
  @Input() display = false;
  @Output() displayChange = new EventEmitter<boolean>();
  @Output() save = new EventEmitter<User>();

  userForm: FormGroup;
  roles: Role[] = [];
  isEditMode = false;

  constructor() {
    this.userForm = this.fb.group({
      id: [null],
      adi: ['', Validators.required],
      soyadi: ['', Validators.required],
      sicil: ['', Validators.required],
      password: [''],
      isAdmin: [false],
      roleId: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadRoles();
    this.setupPasswordValidation();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['user'] && this.user) {
      this.isEditMode = true;
      this.userForm.patchValue(this.user);
      this.userForm.get('password')?.clearValidators();
      this.userForm.get('password')?.updateValueAndValidity();
    } else {
      this.isEditMode = false;
      this.userForm.reset({ isAdmin: false });
      this.userForm.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
      this.userForm.get('password')?.updateValueAndValidity();
    }
  }

  private loadRoles(): void {
    this.roleService.getAllRoles().subscribe((roles: Role[]) => {
      this.roles = roles;
    });
  }

  private setupPasswordValidation(): void {
    // Edit modunda değilse şifre zorunlu
    if (!this.isEditMode) {
      this.userForm.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
    } else {
      this.userForm.get('password')?.clearValidators();
    }
    this.userForm.get('password')?.updateValueAndValidity();
  }
  
  onSave(): void {
    if (this.userForm.valid) {
      const formValue = this.userForm.getRawValue();
      // Edit modunda ve şifre girilmemişse, password alanını gönderme
      if (this.isEditMode && !formValue.password) {
        delete formValue.password;
      }
      this.save.emit(formValue);
      this.closeDialog();
    }
  }

  closeDialog(): void {
    this.display = false;
    this.displayChange.emit(this.display);
    this.userForm.reset();
  }
} 