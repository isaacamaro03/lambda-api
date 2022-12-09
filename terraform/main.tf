terraform {
  backend "s3" {
    bucket = "terraform-backend-0716742c2f2f9"
    key    = "lambda-api/terraformstate"
    region = "us-east-1"
  }
}

resource "aws_s3_bucket" "test-bucket" {
  bucket = "test-bucket-a8f5f167f44f496a4e"

  tags = {
    Name = "My bucket"
  }
}

resource "aws_s3_bucket_acl" "test-bucket-acl" {
  bucket = aws_s3_bucket.test-bucket.id
  acl    = "private"
}
