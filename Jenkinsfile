pipeline {
    agent any
    stages {
        stage("Verify tooling") {
            steps {
                sh '''
                    docker version
                    docker info
                    docker-compose version
                '''
            }
        }
        stage('Start container') {
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
                sh 'docker-compose up --build --wait'
                sh 'docker-compose ps'
            }
        }
    }
}
