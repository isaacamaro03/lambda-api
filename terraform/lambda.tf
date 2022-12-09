resource "aws_iam_role" "lambda-role" {
  name               = "LambdaRole"
  assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      },
      "Effect": "Allow",
      "Sid": ""
    }
  ]
}
EOF
}

data "archive_file" "dummy-lambda-code" {
  type        = "zip"
  output_path = "${path.module}/dummy_lambda_function.zip"
  source_dir  = "${path.module}/src"
}

resource "aws_lambda_function" "minimal-api-lambda" {
  filename      = data.archive_file.dummy-lambda-code.output_path
  function_name = "minimal-api"
  role          = aws_iam_role.lambda-role.arn
  handler       = "lambda-api"
  runtime       = "dotnet6"
  timeout       = 30
}

resource "aws_lambda_function_url" "minimal-api-lambda-url" {
  function_name      = aws_lambda_function.minimal-api-lambda.function_name
  authorization_type = "NONE"
}
