import { LoginReason } from "./login-reason.model";

export class LoginResult {
  get Success(): boolean {
    return this.Reason == LoginReason.Success;
  }
  Reason: LoginReason;
}
