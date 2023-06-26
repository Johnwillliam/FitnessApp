# FitnessApp

kubectl rollout restart deployment fitnessapp
kubectl port-forward --address localhost,10.10.10.69 deployment/fitnessapp 8080

minikube kubectl -- rollout restart deployment fitnessapp
minikube kubectl -- port-forward --address localhost,10.10.10.69 deployment/fitnessapp 8080
