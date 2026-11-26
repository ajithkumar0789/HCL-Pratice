export interface Product {
  id: number;
  name: string;
  category: string;
  description: string;
  price: number;
  imageUrl: string;
  availableSizes: string;
  isFeatured: boolean;
}
export interface CartItem {
  product: Product;
  size: string;
  quantity: number;
}
export interface AuthResponse {
  token: string;
  fullName: string;
  email: string;
}
export interface Order {
  id: number;
  orderDate: string;
  totalAmount: number;
  status: string;
  items: { productName: string; size: string; quantity: number; unitPrice: number }[];
}
