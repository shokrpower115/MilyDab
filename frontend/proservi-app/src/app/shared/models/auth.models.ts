export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterCustomerRequest {
  fullName: string;
  email: string;
  phone: string;
  password: string;
  confirmPassword: string;
  city: string;
  country: string;
  latitude: number;
  longitude: number;
}

export interface RegisterProfessionalRequest {
  fullName: string;
  email: string;
  phone: string;
  password: string;
  confirmPassword: string;
  city: string;
  country: string;
  latitude: number;
  longitude: number;
  specialtyId: number;
  bio: string;
  yearsOfExperience: number;
  hourlyRate: number;
}

export interface AuthResponse {
  token: string;
  user: UserData;
}

export interface UserData {
  id: number;
  email: string;
  fullName: string;
  role: 'Customer' | 'Professional';
  profilePhotoUrl?: string | null;
}
