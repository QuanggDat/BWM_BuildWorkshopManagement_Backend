pipeline {
    agent any
    stages {
        stage('Check container') {
            steps {
                script {
                    try {
                        // Your build commands or steps that might fail
                        sh 'docker stop workshop-management-system-jama'
                        sh 'docker rm workshop-management-system-jama'
                    } catch (Exception e) {
                        echo "Cannot find container workshop-management-system-jama"
                    }
                }
            }
        }
        stage('Start container') {
            steps {
                sh 'docker-compose up --build -d --no-color --wait'
                sh 'docker-compose ps'
            }
        }
    }
}
