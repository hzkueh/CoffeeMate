export interface StepDto {
  id: string
  order: number
  name: string
  description: string
  durationSeconds: number
}

export interface RecipeDto {
  id: string
  title: string
  description: string | null
  steps: StepDto[]
}

export interface CoffeeListDto {
  id: string
  name: string
  description: string
  origin: string
  imageUrl: string | null
}

export interface CoffeeDetailDto extends CoffeeListDto {
  recipe: RecipeDto | null
}
