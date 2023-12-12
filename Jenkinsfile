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
                sh "docker-compose up --build -d"
                sh 'docker-compose ps'
            }
        }
        stage('Remove old images') {
            steps {
                script {
                    try {
						sh 'docker rmi $(docker images -f "dangling=true" -q)'
                    } catch (Exception e) {
                        echo "Warning: Cannot del old image"
                    }
                }
            }
        }
    }
}