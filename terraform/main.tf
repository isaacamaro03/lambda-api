terraform {
  backend "s3" {
    bucket = "terraform-backend-0716742c2f2f9"
    key    = "lambda-api/terraformstate"
    region = "us-east-1"
  }
}
